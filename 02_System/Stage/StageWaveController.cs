using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageWaveController
{
    private StageWaveData _nowContinuousWave;
    public StageWaveData NowContinuousWave => _nowContinuousWave;
    private float _nowSpawnDelay = 0;

    public event Action<MonsterTypeData> SpawnWaveMonsterAction;
    public event Action<MonsterTypeData> SpawnBossMonsterAction;
    
    
    public void EnterWave(StageWaveData wave)
    {
        _nowSpawnDelay = 0;
        
        if (wave.WaveType == WaveType.Continuous||
            wave.WaveType == WaveType.Super)
        {
            _nowContinuousWave = wave;
        }
        else if (wave.WaveType == WaveType.Boss)
        {
            _nowContinuousWave = null;
            SpawnBossMonster(wave);
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
        if (_nowContinuousWave == null) return;

        _nowSpawnDelay += Time.deltaTime;
        
        if (_nowContinuousWave.SpawnDelay <= _nowSpawnDelay)
        {
            SpawnMonster();
            _nowSpawnDelay = 0;
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
            SpawnBossMonsterAction?.Invoke(bossWave.MonsterTypeData[i]);
        }
    }
    
    // boss 죽었는지 확인하고 다음 wave 가야해
}