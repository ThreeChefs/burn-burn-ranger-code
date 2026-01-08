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

    //public event Func<MonsterTypeData, Monster> SpawnWaveMonsterAction;
    //public event Func<MonsterTypeData, Monster> SpawnBossMonsterAction;
    public event Action OnStageEndAction;
    
    
    private List<Monster> _nowBossMonsters;


    public StageWaveController(StageData nowStageData)
    {
        if (nowStageData == null) return;

        _nowBossMonsters  = new List<Monster>();
        
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
            if (_nowBossMonsters != null)
            {
                //_nowBossMonster.
            }
        }
    }

    void SpawnMonster()
    {
        if (_nowContinuousWave == null) return;
        if (_nowContinuousWave.Monsters == null) return;
        if (_nowContinuousWave.Monsters.Count == 0) return;

        for (int i = 0; i < _nowContinuousWave.SpawnCount; ++i)
        {
            int monsterIdx = Define.Random.Next(0, _nowContinuousWave.Monsters.Count);

            MonsterPoolManager.Instance.SpawnWaveMonster(_nowContinuousWave.Monsters[monsterIdx]);
        }
    }

    void SpawnBossMonster(StageWaveData bossWave)
    {
        for (int i = 0; i < bossWave.Monsters.Count; ++i)
        {
            Monster spawnedMonster = MonsterPoolManager.Instance.SpawnBossMonster(_nowContinuousWave.Monsters[i]);

            if (spawnedMonster != null)
            {
                _nowBossMonsters.Add(spawnedMonster);
                spawnedMonster.onDieAction += OnDieBossMonster;
            }
            
        }
    }

    void OnDieBossMonster(Monster monster)
    {
        if (_nowBossMonsters.Contains(monster))
        {
            _nowBossMonsters.Remove(monster);
        }
        
        // 보스가 모두 죽었다면 다음 웨이브
        // 다음 웨이브가 없으면 스테이지 종료
        if (_nowBossMonsters.Count == 0)
        {
            if (_waveQueue.Count > 0)
            {
                EnterWave(_waveQueue.Dequeue().StageWaveData);
            }
            else
            {
                Logger.Log("스테이지 클리어");
                OnStageEndAction?.Invoke();
            }
        }

        monster.onDieAction -= OnDieBossMonster;
    }

}