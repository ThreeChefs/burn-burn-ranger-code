using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageWaveController
{
    public float PlayeTime => _playTime;
    private float _playTime = 0;

    private StageWaveData _nowContinuousWave;
    public StageWaveData NowContinuousWave => _nowContinuousWave;

    private StageWaveData _nowWave;
    public StageWaveData NowWave => _nowWave;

    private float _nowSpawnDelay = 0;


    // todo : WaveQueue 도 WaveController 에 집어넣어놓기
    Queue<StageWaveEntry> _waveQueue = new Queue<StageWaveEntry>();

    public event Func<MonsterTypeData, Monster> SpawnWaveMonsterAction;
    public event Func<MonsterTypeData, Monster> SpawnBossMonsterAction;
    private Monster _nowBossMonster;
    

    public StageWaveController(StageData nowStageData)
    {
        if (nowStageData == null) return;

        _waveQueue = new Queue<StageWaveEntry>();
        for (int i = 0; i < nowStageData.StageWaves.Count; i++)
        {
            _waveQueue.Enqueue(nowStageData.StageWaves[i]);
        }
        
        EnterWave(_waveQueue.Dequeue().StageWaveData);
    }

    public void EnterWave(StageWaveData wave)
    {
        _nowWave = wave;
        _nowSpawnDelay = 0;

        switch (wave.WaveType)
        {
            case WaveType.Continuous:
                _nowContinuousWave = wave;
                break;

            case WaveType.Super:
                _nowContinuousWave = wave;
                break;

            case WaveType.MiniBoss:
                break;

            case WaveType.Boss:
                SpawnBossMonster(wave);
                break;
        }

    
        if (_nowContinuousWave != null)
        {
            switch (_nowContinuousWave.WaveType)
            {
                case WaveType.Super:
                    // todo : 몬스터가 몰려옵니다~
                    break;

                case WaveType.Boss:
                    // todo : 보스 몬스터 알림
                    break;
            }
        }
    }

    public void Update()
    {
        // 플레이 시간 갱신
        if (_nowWave.WaveType != WaveType.Boss)
        {
            _playTime += Time.deltaTime;
            
            // 상시 스폰 웨이브일 때, 스폰 시간 갱신
            if (_nowContinuousWave.WaveType == WaveType.Continuous || _nowContinuousWave.WaveType == WaveType.Super)
            {
                _nowSpawnDelay += Time.deltaTime;

                if (_nowContinuousWave != null)
                {
                    if (_nowContinuousWave.SpawnDelay <= _nowSpawnDelay)
                    {
                        SpawnMonster();
                        _nowSpawnDelay = 0;
                    }
                }
            }
            
            // 다음 웨이브 시간 확인
            if (_waveQueue.Count > 0)
            {
                if (_waveQueue.Peek().WaveStartTime <= _playTime)
                {
                    StageWaveEntry nextWaveData = _waveQueue.Dequeue();
                    EnterWave(nextWaveData.StageWaveData);
                }
            }
            
        }
        else
        {
            if (_nowBossMonster != null)
            {
                //_nowBossMonster.
            }
        }
        
        
        
       

     
    }

    void SpawnMonster()
    {
        if (_nowContinuousWave == null) return;
        if (_nowContinuousWave.MonsterTypeData == null) return;
        if (_nowContinuousWave.MonsterTypeData.Count == 0) return;

        for (int i = 0; i < _nowContinuousWave.SpawnCount; ++i)
        {
            int monsterIdx = Random.Range(0, _nowContinuousWave.MonsterTypeData.Count);
            SpawnWaveMonsterAction?.Invoke(_nowContinuousWave.MonsterTypeData[monsterIdx]);
        }
    }

    void SpawnBossMonster(StageWaveData bossWave)
    {
        for (int i = 0; i < bossWave.MonsterTypeData.Count; ++i)
        {
            _nowBossMonster = SpawnBossMonsterAction?.Invoke(bossWave.MonsterTypeData[i]);
        }
    }

    // boss 죽었는지 확인하고 다음 wave 가야해
}