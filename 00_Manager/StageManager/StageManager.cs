using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : SceneSingletonManager<StageManager>
{
    [SerializeField] private SoDatabase _stageDataBase;
    [SerializeField] private SoDatabase _skillDataBase;     // todo : 역시 다른 곳에 SO를 몰아두는게 낫지 않을지?! 
    private List<StageData> _stageDatas = new List<StageData>();

    // 스테이지 맵을 생성해주는 거
    // 화면 내 맵을 들고 있어야하는데
    
    private StagePlayer _player;
    StageData _nowStage;
    private StageWaveController _waveController;

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
    

    List<Monster> _spawnedMonsters = new List<Monster>();


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public override void Init()
    {
        // 어떻게 꽂아 넣을지 고민 필요
        // 플레이어를 생성하면 좋을 것 같음.
        _stageDatas = _stageDataBase.GetDatabase<StageData>(); // Database 만 넣어둔 애 들고다니면 곤란할까요
        
    }

    bool SetStageData(int stageNum)
    {
        if (_stageDatas.Count <= stageNum)
        {
            Logger.Log("스테이지 없읍!");
            return false;
        }

        // todo : Pool 적용 시 스테이지데이터 읽고 사용할 몬스터들 등록 필요

        _nowStage = _stageDatas[stageNum];


        if (_waveController != null)
        {
            _waveController.SpawnBossMonsterAction -= SpawnBossMonster;
            _waveController.SpawnWaveMonsterAction -= SpawnWaveMonster;
        }

        _waveController = new StageWaveController(_nowStage);
        _waveController.SpawnBossMonsterAction += SpawnBossMonster;
        _waveController.SpawnWaveMonsterAction += SpawnWaveMonster;
        _waveController.OnStageEndAction += () => { Logger.Log("스테이지클리어"); };

        return true;
    }

    private void Start()
    {
        // 플레이어 생성
        _player = PlayerManager.Instance.SpawnPlayer();
        _player.OnDieAction += GameOver;
        _skillSystem = new SkillSystem(_skillDataBase, _player);
        
        if (IsTest) return;
        
        SetStageData(GameManager.Instance.SelectedStageNumber - 1);
        GameStart();
    }

    private void Update()
    {
        #if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.Scene.ReLoadSceneAsync();
        }
        
        #endif
        
        
        if (_isPlaying == false) return;
        _waveController?.Update();
    }

    void GameStart()
    {
        _isPlaying = true;
        OnGameStartAction?.Invoke();
    }

    public void GameClear()
    {
        OnGameClearAction?.Invoke();
    }
    
    public void GameOver()
    {
        OnGameOverAction?.Invoke();
    }


    public Monster SpawnWaveMonster(MonsterTypeData monsterTypeData)
    {
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0;
        dir.Normalize();

        Vector3 randomPos = _player.transform.position + (dir * Define.RandomRange(Define.MinMonsterSpawnDistance, Define.MaxMonsterSpawnDistance));
        randomPos.z = 0;
        GameObject monster = Instantiate(monsterTypeData.prefab, randomPos, Quaternion.identity);

        if (monster.TryGetComponent(out Monster monsterComponent))
        {
            monsterComponent.ApplyData(monsterTypeData);
            _spawnedMonsters.Add(monsterComponent);
            monsterComponent.onDieAction += DestroyMonster; // todo: Pool 적용하면 매번 넣지 않게 처리하기 
            return monsterComponent;
        }
        // 화면에 보이는 범위를 가져와야할 듯
        // 벽이 있을 수 있으니 스폰 가능한 곳도 있어야 함.

        return null;
    }

    public Monster SpawnBossMonster(MonsterTypeData monsterTypeData)
    {
        // 위치 지정 필요
        // 일단은 그냥 스폰

        //todo PoolManager에 DeactiveAll... 있음
        for (int i = 0; i < _spawnedMonsters.Count; i++)
        {
            Destroy(_spawnedMonsters[i].gameObject);
        }

        _spawnedMonsters.Clear();

        return SpawnWaveMonster(monsterTypeData);
    }

    public void DestroyMonster(Monster monster)
    {
        _killCount += 1;
        
        if (_spawnedMonsters.Contains(monster))
        {
            _spawnedMonsters.Remove(monster);
        }

        Destroy(monster.gameObject);
    }

    // todo : 몬스터말고 상자같은 애가 나올 수 있음. 내부 로직은 변경 예정 / 함수이름도 바꿀 예정
    public Transform GetNearestMonster()
    {
        if(_spawnedMonsters.Count == 0)
            return null;

        if (_player == null) return null;
        
        Monster nearestMonster = _spawnedMonsters[0];
        float distance = Vector2.Distance(nearestMonster.transform.position, _player.transform.position);
        
        for (int i = 1; i < _spawnedMonsters.Count; i++)
        {
            float nowDistance = Vector2.Distance(nearestMonster.transform.position, _spawnedMonsters[i].transform.position);
            if (distance >= nowDistance)
            {
                nearestMonster = _spawnedMonsters[i];
                distance = nowDistance;
            }
        }
        return nearestMonster.transform;
    }
    

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