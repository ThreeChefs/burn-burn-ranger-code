using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageWaveController
{
    private StageWaveData _nowWave;
    public StageWaveData nowWave => _nowWave;
    private float _nowSpawnDelay = 0;

    public event Action<MonsterTypeData> SpawnWaveMonsterAction;
    public event Action<MonsterTypeData> SpawnBossMonsterAction;
    
    
    public void EnterWave(StageWaveData wave)
    {
        _nowWave = wave;
        _nowSpawnDelay = 0;

        switch (_nowWave.WaveType)
        {
            case WaveType.Super:
                // todo : 몬스터가 몰려옵니다~
                break;

            case WaveType.Boss:
                // todo : 보스 몬스터 알림
                break;
        }
    }

    public void Update()
    {
        if (_nowWave == null) return;

        _nowSpawnDelay += Time.deltaTime;

        if (_nowWave.SpawnDelay >= _nowWave.SpawnDelay)
        {
            SpawnMonster();
            _nowSpawnDelay = 0;
        }
    }

    void SpawnMonster()
    {
        if (_nowWave.MonsterTypeData == null) return;
        if (_nowWave.MonsterTypeData.Count == 0) return;
        
        if (_nowWave.WaveType == WaveType.Boss)
        {
            for (int i = 0; i < _nowWave.MonsterTypeData.Count; ++i)
            {
                SpawnWaveMonsterAction?.Invoke(_nowWave.MonsterTypeData[i]);
            }
        }
        else
        {
            for (int i = 0; i < _nowWave.SpawnCount; ++i)
            {
                int monsterIdx = Random.Range(0, _nowWave.MonsterTypeData.Count);
                SpawnBossMonsterAction?.Invoke(_nowWave.MonsterTypeData[monsterIdx]);
                
            }
        }

    }
}