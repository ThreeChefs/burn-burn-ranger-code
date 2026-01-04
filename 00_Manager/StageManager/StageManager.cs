using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : SceneSingletonManager<StageManager>
{
    [SerializeField] private SoDatabase _stageDataBase;
    private List<StageData> _stageDatas = new List<StageData>();

    // 스테이지 맵을 생성해주는 거
    // 화면 내 맵을 들고 있어야하는데
    
    
    private StagePlayer _player;
    StageData _nowStage;
    private StageWaveController _waveController;
    
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
    
    public event Action OnGameStartAction;
    public event Action OnGameEndAction;
    
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
        _stageDatas = _stageDataBase.GetDatabase<StageData>();      // Database 만 넣어둔 애 들고다니면 곤란할까요

        _player = PlayerManager.Instance.SpawnPlayer();
        
    }
    
    bool SetStageData(int stageNum)
    {
        if (_stageDatas.Count < stageNum)
        {
            Logger.Log("스테이지 없읍!");
            return false;
        }
            
        _nowStage =_stageDatas[stageNum];


        if (_waveController != null)
        {
            _waveController.SpawnBossMonsterAction -= SpawnBossMonster;
            _waveController.SpawnWaveMonsterAction -= SpawnWaveMonster;
        }
        
        _waveController = new StageWaveController(_nowStage);
        _waveController.SpawnBossMonsterAction += SpawnBossMonster;
        _waveController.SpawnWaveMonsterAction += SpawnWaveMonster;
        
        // _waveQueue = new Queue<StageWaveEntry>();
        // for (int i = 0; i < _nowStage.StageWaves.Count; i++)
        // {
        //     _waveQueue.Enqueue(_nowStage.StageWaves[i]);
        // }

        return true;
    }

    private void Start()
    {
        if (IsTest) return;
        
        // todo : 이전 Scene에서 선택한 스테이지번호 넘겨주기
        SetStageData(0);
        GameStart();
    }
    
    private void Update()
    {
        if (_isPlaying == false) return;
        
        _waveController?.Update();
        
    }

    void GameStart()
    {
        _isPlaying = true;
        OnGameStartAction?.Invoke();
    }

    public void GameEnd(bool isClear)
    {
        OnGameEndAction?.Invoke();
    }
    
    
    public Monster SpawnWaveMonster(MonsterTypeData monsterTypeData)
    {
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0;
        dir.Normalize();
        
        Vector3 randomPos = _player.transform.position + (dir * Random.Range(Define.MinMonsterSpawnDistance, Define.MaxMonsterSpawnDistance));
        GameObject monster = Instantiate(monsterTypeData.prefab, randomPos, Quaternion.identity);
        
        if (monster.TryGetComponent(out Monster monsterComponent))
        {
            monsterComponent.ApplyData(monsterTypeData);
            _spawnedMonsters.Add(monsterComponent);
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
    
}