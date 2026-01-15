using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageWaveController
{
    public float PlayeTime => _playTime;
    private float _playTime = 0;

    private StageWaveEntry _nowContinuousWave;
    public StageWaveEntry NowContinuousWave => _nowContinuousWave;

    private StageWaveEntry _nowWave;
    public StageWaveEntry NowWave => _nowWave;

    private float _nowSpawnDelay = 0;


    // todo : WaveQueue 도 WaveController 에 집어넣어놓기
    Queue<StageWaveEntry> _waveQueue = new Queue<StageWaveEntry>();
    public event Action OnStageEndAction;
       
    private List<Monster> _nowBossMonsters;

    private int _saveExp = 0;
    public int SaveExp => _saveExp;

    static float _itemBoxSpawnStartTime = 20; // 스테이지 시작 후 20초 뒤부터 박스 스폰
    static float _itemBoxSpawnInterval = 10; // 10초마다 박스 스폰
    int _itemBoxSpawnCount = 0;


    public StageWaveController(StageData nowStageData)
    {
        if (nowStageData == null) return;

        _nowBossMonsters  = new List<Monster>();
        
        _waveQueue = new Queue<StageWaveEntry>();
        for (int i = 0; i < nowStageData.StageWaves.Count; i++)
        {
            _waveQueue.Enqueue(nowStageData.StageWaves[i]);
        }


        // 사용할 풀 로드

        for (int i = 0; i < nowStageData.StageWaves.Count; ++i)
        {
            StageWaveData waveData = nowStageData.StageWaves[i].WaveData;
            
            if (waveData == null) continue;
            if (waveData.Monsters == null) continue;
            
            for (int j = 0; j < waveData.Monsters.Count; ++j)
            {
                MonsterPoolIndex poolIndex = waveData.Monsters[j];

                MonsterManager.Instance.UsePool(poolIndex);
            }
        }


        // box 도 미리 로드
        MonsterManager.Instance.UsePool(MonsterPoolIndex.ItemBox);

        EnterWave(_waveQueue.Dequeue());
    }

    public void EnterWave(StageWaveEntry wave)
    {
        if(_nowWave != null)
        {
            // 이전 웨이브 클리어 보상 저장
            _saveExp += _nowWave.WaveClearExp;
            PlayerManager.Instance.StagePlayer.AddGold(_nowWave.WaveClearGold);
        }

        _nowWave = wave;
        _nowSpawnDelay = 0;

        switch (wave.WaveData.WaveType)
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
                SpawnBossMonster(wave.WaveData);
                break;
        }


        if (_nowContinuousWave != null)
        {
            switch (_nowContinuousWave.WaveData.WaveType)
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
        if (_nowWave.WaveData.WaveType != WaveType.Boss)
        {
            _playTime += Time.deltaTime;

            // 상시 스폰 웨이브일 때, 스폰 시간 갱신
            if (_nowContinuousWave.WaveData.WaveType == WaveType.Continuous || _nowContinuousWave.WaveData.WaveType == WaveType.Super)
            {
                _nowSpawnDelay += Time.deltaTime;

                if (_nowContinuousWave != null)
                {
                    if (_nowContinuousWave.WaveData.SpawnDelay <= _nowSpawnDelay)
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
                    EnterWave(nextWaveData);
                }
            }
        }
        else
        {
            if (_nowBossMonsters != null)
            {
            }
        }

        UpdateBoxSpawn();
    }


    void UpdateBoxSpawn()
    {
        if (_playTime >= _itemBoxSpawnStartTime + (_itemBoxSpawnInterval * _itemBoxSpawnCount))
        {
            MonsterManager.Instance.SpawnWaveMonster(MonsterPoolIndex.ItemBox);
            _itemBoxSpawnCount++;
        }
    }


    void SpawnMonster()
    {
        if (_nowContinuousWave == null) return;
        if (_nowContinuousWave.WaveData.Monsters == null) return;
        if (_nowContinuousWave.WaveData.Monsters.Count == 0) return;

        for (int i = 0; i < _nowContinuousWave.WaveData.SpawnCount; ++i)
        {
            int monsterIdx = Define.Random.Next(0, _nowContinuousWave.WaveData.Monsters.Count);

            MonsterManager.Instance.SpawnWaveMonster(_nowContinuousWave.WaveData.Monsters[monsterIdx]);
        }
    }

    void SpawnBossMonster(StageWaveData bossWave)
    {
        for (int i = 0; i < bossWave.Monsters.Count; ++i)
        {
            Monster spawnedMonster = MonsterManager.Instance.SpawnBossMonster(bossWave.Monsters[i]);

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
                EnterWave(_waveQueue.Dequeue());
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