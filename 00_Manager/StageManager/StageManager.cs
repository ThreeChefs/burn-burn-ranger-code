using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SceneSingletonManager<StageManager>
{
    [SerializeField] private SoDatabase _skillDataBase;     // memo : 역시 다른 곳에 SO를 몰아두는게 낫지 않을지?! 
    private List<StageData> _stageDatas = new List<StageData>();

    StageData _nowStage;
    public int NowStageNum { get; private set; }

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




    // 액션
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


    bool SetStageData()
    {
        NowStageNum = GameManager.Instance.StageProgress.LastSelectedStageNum;
        int stageIndex = NowStageNum - 1;

        if (_stageDatas.Count <= stageIndex || stageIndex < 0)
        {
            Logger.Log("스테이지 없음!");
            return false;
        }

        _nowStage = _stageDatas[stageIndex];

        if (_stageDatas[stageIndex].Map != null)
        {
            Instantiate(_stageDatas[stageIndex].Map);
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

        // 카메라 세팅
        if (Camera.main.TryGetComponent<FollowCamera>(out var camera))
        {
            camera.ConnectPlayer();
        }

        // 게임 시작
        SetStageData();
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        GameStart();
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.Scene.ReLoadSceneAsync();
        }
        // 테스트용
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnSkillSelectUI(_player.StageLevel.Level);
        }
        if (_isPlaying == false) return;
        _waveController?.Update();
    }


    #region 이벤트
    public void OnDieMonster(Monster monster)
    {
        _killCount += 1;
        AddKillCountAction?.Invoke(_killCount);
    }

    void SpawnSkillSelectUI(int level)
    {
        PauseGame();
        UIManager.Instance.ShowUI(UIName.UI_SkillSelect);
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

        // 보상 지급
        PlayerManager.Instance.StagePlayer.AddGold(_nowStage.RewardGold);   // 스테이지 클리어 보상 추가
        PlayerManager.Instance.StagePlayer.UpdateGold();
        PlayerManager.Instance.Condition.GlobalLevel.AddExp(_nowStage.RewardExp + _waveController.SaveExp);
        List<StageRewardInfo> rewards = GiveReward();

        StageResultUI resultUI = (StageResultUI)UIManager.Instance.SpawnUI(UIName.UI_Victory);
        if (resultUI != null)
        {
            StageResultInfo resultInfo = new StageResultInfo
            {
                stageName = _nowStage.StageName,
                playTIme = PlayTime,
                killCount = KillCount,
                gold = PlayerManager.Instance.StagePlayer.GoldValue + +_nowStage.RewardGold,
                exp = _waveController.SaveExp + _nowStage.RewardExp,
            };

            resultUI.Init(resultInfo, rewards);
        }


        // 스테이지 진행 정보 저장
        GameManager.Instance.StageProgress.SaveStagePrgressNum(NowStageNum + 1, (int)PlayTime);
    }

    public void GameOver()
    {
        OnGameOverAction?.Invoke();
        PauseGame();

        // 보상 지급
        PlayerManager.Instance.StagePlayer.UpdateGold();
        PlayerManager.Instance.Condition.GlobalLevel.AddExp(_waveController.SaveExp);   // 쌓인 경험치만 지급


        // UI 표시
        StageResultUI resultUI = (StageResultUI)UIManager.Instance.SpawnUI(UIName.UI_Defeat);
        if (resultUI != null)
        {
            StageResultInfo resultInfo = new StageResultInfo
            {
                stageName = _nowStage.StageName,
                playTIme = PlayTime,
                killCount = KillCount,
                gold = PlayerManager.Instance.StagePlayer.GoldValue,
                exp = _waveController.SaveExp,
            };

            // todo : GameOver 했을 때에도 모아둔 보상을 까서 전달해야함
            resultUI.Init(resultInfo, null);
        }

        // 스테이지 진행 정보 저장
        GameManager.Instance.StageProgress.SaveStagePrgressNum(NowStageNum, (int)PlayTime);
    }


    // 스테이지 보상 제공
    List<StageRewardInfo> GiveReward()
    {
        List<StageRewardInfo> rewardInfos = new List<StageRewardInfo>();

        StageRewardInfo weaponMaterial = new StageRewardInfo { type = ItemType.UpgradeMaterial, upgradeMaterialType = WalletType.UpgradeMaterial_Weapon };
        StageRewardInfo armortMaterial = new StageRewardInfo { type = ItemType.UpgradeMaterial, upgradeMaterialType = WalletType.UpgradeMaterial_Armor };
        StageRewardInfo shoesMaterial = new StageRewardInfo { type = ItemType.UpgradeMaterial, upgradeMaterialType = WalletType.UpgradeMaterial_Shoes };
        StageRewardInfo glovesMaterial = new StageRewardInfo { type = ItemType.UpgradeMaterial, upgradeMaterialType = WalletType.UpgradeMaterial_Gloves };
        StageRewardInfo beltMaterial = new StageRewardInfo { type = ItemType.UpgradeMaterial, upgradeMaterialType = WalletType.UpgradeMaterial_Belt };
        StageRewardInfo necklaceMaterial = new StageRewardInfo { type = ItemType.UpgradeMaterial, upgradeMaterialType = WalletType.UpgradeMaterial_Necklace };

        for (int i = 0; i < _nowStage.RewardBoxCount; i++)
        {
            float rand = UnityEngine.Random.value;
            StageRewardInfo newRewardInfo = default;

            if (rand <= StageDefine.StageClearEquipRewardWeight)
            {
                // 장비 주기
                newRewardInfo.type = ItemType.Equipment;

                newRewardInfo.itemInfo = GameManager.Instance.PickUpSystem.PickUp(0);
                PlayerManager.Instance.Inventory.Add(newRewardInfo.itemInfo);
                
                newRewardInfo.count += 1;

                rewardInfos.Add(newRewardInfo);

            }
            else
            {
                WalletType randomUpgradeMaterial = (WalletType)Define.Random.Next((int)WalletType.UpgradeMaterial_Weapon, (int)WalletType.UpgradeMaterial_Weapon + StageDefine.EquipTypeCount);


                switch (randomUpgradeMaterial)
                {
                    case WalletType.UpgradeMaterial_Weapon:
                        weaponMaterial.count += 1;
                        break;
                    case WalletType.UpgradeMaterial_Armor:
                        armortMaterial.count += 1;
                        break;
                    case WalletType.UpgradeMaterial_Shoes:
                        shoesMaterial.count += 1;
                        break;
                    case WalletType.UpgradeMaterial_Gloves:
                        glovesMaterial.count += 1;
                        break;
                    case WalletType.UpgradeMaterial_Belt:
                        beltMaterial.count += 1;
                        break;
                    case WalletType.UpgradeMaterial_Necklace:
                        necklaceMaterial.count += 1;
                        break;
                }
            }
        }

        if (weaponMaterial.count > 0) rewardInfos.Add(weaponMaterial);
        if (armortMaterial.count > 0) rewardInfos.Add(armortMaterial);
        if (shoesMaterial.count > 0) rewardInfos.Add(shoesMaterial);
        if (glovesMaterial.count > 0) rewardInfos.Add(glovesMaterial);
        if (beltMaterial.count > 0) rewardInfos.Add(beltMaterial);
        if (necklaceMaterial.count > 0) rewardInfos.Add(necklaceMaterial);

        for (int i = 0; i < rewardInfos.Count; ++i)
        {
            // 장비 어떻게 주면 되는지?

            if (rewardInfos[i].type == ItemType.UpgradeMaterial)
                PlayerManager.Instance.Wallet[rewardInfos[i].upgradeMaterialType].Add(rewardInfos.Count);
        }


        return rewardInfos;
    }

    #endregion


}

