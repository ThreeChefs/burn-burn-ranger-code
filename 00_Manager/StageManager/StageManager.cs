using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SceneSingletonManager<StageManager>
{
    public Player Player => _player;
    private Player _player;

    // 스테이지 맵을 생성해주는 거
    // 몬스터 소환 조건
    // 몬스터 소환
    // 화면 내 맵을 들고 있어야하는데
    // 웨이브에 대한 것도 만들어야 하네
    
    [SerializeField] private SoDatabase _stageDataBase;
    private List<StageData> _stageDatas = new List<StageData>();
    
    Queue<StageWaveData> _waveQueue = new Queue<StageWaveData>();
    
    StageData _nowStage;
    
    private StageWaveController _waveController;

    public float PlayTime => _playTime;
    private float _playTime = 0;

    private bool _isPlaying = false;
    public bool IsPlaying => _isPlaying;
    

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public override void Init()
    {
        // 어떻게 꽂아 넣을지 고민 필요
        // 플레이어를 생성하면 좋을 것 같음.
        _player = FindObjectOfType<Player>();
        _stageDatas = _stageDataBase.GetDatabase<StageData>();      // Database 만 넣어둔 애 들고다니면 곤란할까요
        
        SetStageData(0);
    }
    
    void SetStageData(int stageNum)
    {
        _nowStage =_stageDatas[stageNum];
        _waveQueue = new Queue<StageWaveData>();
        for (int i = 0; i < _nowStage.MonsterWaves.Count; i++)
        {
            _waveQueue.Enqueue(_nowStage.MonsterWaves[i]);
        }
    }

    private void Start()
    {
        //언제 게임 시작하지
        GameStart();
    }
    
    private void Update()
    {
        if (_isPlaying == false) return;
        
        _playTime += Time.deltaTime;
        
        WaveUpdate();
    }

    void GameStart()
    {
        _isPlaying = true;
    }

    public void GameEnd(bool isClear)
    {
            
    }
    
    void WaveUpdate()
    {
        if (_waveQueue.Peek().WaveStartTime <= _playTime)
        {
            _waveController.EnterWave(_waveQueue.Dequeue());
        }
        
        _waveController.Update();
    }
    
    
    public void SpawnWaveMonster(MonsterTypeData monsterTypeData)
    {   
        // 화면에 보이는 범위를 가져와야할 듯
        // 벽이 있을 수 있으니 스폰 가능한 곳도 있어야 함.
        
    }
    public void SpawnBossMonster(MonsterTypeData monsterTypeData)
    {
        // 위치 지정
    }
    
    
}