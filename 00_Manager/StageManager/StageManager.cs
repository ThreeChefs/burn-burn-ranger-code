using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : SceneSingletonManager<StageManager>
{
    //[SerializeField] private SoDatabase _stageDataBase;
    [SerializeField] private SoDatabase _skillDataBase;     // todo : 역시 다른 곳에 SO를 몰아두는게 낫지 않을지?! 
    private List<StageData> _stageDatas = new List<StageData>();

    // 스테이지 맵을 생성해주는 거
    // 화면 내 맵을 들고 있어야하는데

    StageData _nowStage;
    public int NowStageNumber { get; private set; }

    private StageWaveController _waveController;
    private StagePlayer _player;

    public SkillSystem SkillSystem => _skillSystem;
    SkillSystem _skillSystem;

    public float PlayTime
    {
        get
        {
            if (_waveController == null) return 0f;
            else return _waveController.PlayeTime;
        }
    }

    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;

    private int _killCount = 0;
    public int KillCount => _killCount;

    public event Action OnGameStartAction;
    public event Action OnGameOverAction;
    public event Action OnGameClearAction;
    public event Action<int> AddKillCountAction;


    protected override void Awake()
    {
        Time.timeScale = 1;
        base.Awake();
        Init();
    }

    public override void Init()
    {
        _stageDatas = GameManager.Instance.StageDatabase;
    }

    bool SetStageData(int stageNum)
    {
        NowStageNumber = stageNum;

        if (_stageDatas.Count <= stageNum)
        {
            Logger.Log("스테이지 없음!");
            return false;
        }

        // todo : Pool 적용 시 스테이지데이터 읽고 사용할 몬스터들 등록 필요
        _nowStage = _stageDatas[stageNum];

        if (_stageDatas[stageNum].Map != null)
        {
            Instantiate(_stageDatas[stageNum].Map);
        }


        _waveController = new StageWaveController(_nowStage);
        _waveController.OnStageEndAction += GameClear;

        return true;
    }

    private void Start()
    {
        // 플레이어 생성
        _player = PlayerManager.Instance.SpawnPlayer();
        _player.OnDieAction += GameOver;

        // 플레이어 이벤트 연결
        _player.StageLevel.OnLevelChanged += SpawnSkillSelectUI;
        UIManager.Instance.LoadUI(UIName.UI_Stage);

        // 스킬 시스템 생성
        _skillSystem = new SkillSystem(_skillDataBase, _player);



        // todo : 
        // 카메라 세팅
        // 나중에 맵이랑 연결해줘야함 
        if (Camera.main.TryGetComponent<FollowCamera>(out var camera))
        {
            camera.ConnectPlayer();
        }

        // 게임 시작
        if (IsTest) return;
        SetStageData(GameManager.Instance.SelectedStageNumber - 1);
        GameStart();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.Scene.ReLoadSceneAsync();
        }
        if (_isPlaying == false) return;
        _waveController?.Update();
    }


    #region 이벤트

    void SpawnSkillSelectUI(int level)
    {
        PauseGame();
        UIManager.Instance.SpawnUI(UIName.UI_SkillSelect);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void GameStart()
    {
        _isPlaying = true;
        OnGameStartAction?.Invoke();
    }

    public void GameClear()
    {
        OnGameClearAction?.Invoke();
        PauseGame();

        StageResultUI resultUI = (StageResultUI)UIManager.Instance.SpawnUI(UIName.UI_Victory);
        if (resultUI != null)
        {
            resultUI.Init(0, _waveController.SaveExp);
        }

        // 보상 지급
        PlayerManager.Instance.StagePlayer.AddGold(_nowStage.RewardGold);   // ㅅ테이지 클리어 보상 추가
        PlayerManager.Instance.StagePlayer.UpdateGold();
        PlayerManager.Instance.Condition.GlobalLevel.AddExp(_nowStage.RewardExp + _waveController.SaveExp);
    }

    public void GameOver()
    {
        OnGameOverAction?.Invoke();
        PauseGame();

        StageResultUI resultUI = (StageResultUI)UIManager.Instance.SpawnUI(UIName.UI_Defeat);
        if (resultUI != null)
        {
            resultUI.Init(0, _waveController.SaveExp);
        }

        // 보상 지급
        PlayerManager.Instance.StagePlayer.UpdateGold();
        PlayerManager.Instance.Condition.GlobalLevel.AddExp(_waveController.SaveExp);   // 쌓인 경험치만 지급
    }

    #endregion

    #region  몬스터

    public void OnDieMonster(Monster monster)
    {
        _killCount += 1;
        AddKillCountAction?.Invoke(_killCount);
    }

    public Transform GetNearestMonster()
    {
        return MonsterManager.Instance.GetNearestMonster();
    }


    #endregion


    #region Test
    [Title("Test")]
    public bool IsTest = false;
    public int TestStageNum = 0;

    [Button("Test Stage Start")]
    public void TestStart()
    {
        if (_isPlaying)
        {
            Logger.Log("이미 플레이 중");
            return;
        }

        if (SetStageData(TestStageNum))
        {
            GameStart();
        }
    }

    #endregion
}