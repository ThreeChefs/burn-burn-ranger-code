using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

// 몬스터가 맵에 차 있어야하는 양이 있고 그거에 맞게 나오는 것도 좋을 것 같음

public class StageWaveController
{
    private float _playTime = 0;
    private int _saveExp = 0;

    private StageWaveEntry _nowContinuousWave;
    private StageWaveEntry _nowWave;

    List<float> _continuousSpawnDelays = new List<float>();

    Queue<StageWaveEntry> _waveQueue = new Queue<StageWaveEntry>();
    private List<Monster> _nowBossMonsters;


    // 이벤트
    public event Action OnStageEndAction;


    // 웨이브 진행 캐싱 값
    int _itemBoxSpawnCount = 0;
    bool _readyWarnningSign = false;


    // 고정 값
    static float _itemBoxSpawnStartTime = 20; // 스테이지 시작 후 20초 뒤부터 박스 스폰
    static float _itemBoxSpawnInterval = 10; // 10초마다 박스 스폰
    readonly string _bossWarnningSignText = "보스 접근 중!";     // warnningsign에 박아둘까
    readonly string _superWaveWarnningSignText = "몬스터가 몰려옵니다!";

    static float _defaultOrthoSize = 12f;
    static float _superWaveOrthoSize = 16f;


    #region public 프로퍼티

    public int SaveExp => _saveExp;
    public float PlayeTime => _playTime;

    #endregion


    #region 초기화 - StageData 읽고 Stage 준비
    public StageWaveController(StageData nowStageData)
    {
        if (nowStageData == null) return;

        _nowBossMonsters = new List<Monster>();

        _waveQueue = new Queue<StageWaveEntry>();
        for (int i = 0; i < nowStageData.StageWaves.Count; i++)
        {
            _waveQueue.Enqueue(nowStageData.StageWaves[i]);
        }


        // 사용할 몬스터 로드
        for (int i = 0; i < nowStageData.StageWaves.Count; ++i)
        {
            switch (nowStageData.StageWaves[i].WaveType)
            {
                case WaveType.Continuous:
                case WaveType.Super:
                    MonsterSpawnInfo[] monsterSpawnInfo = nowStageData.StageWaves[i].ContinuouseMOnsterSpawnInfos;
                    for (int j = 0; j < monsterSpawnInfo.Length; ++j)
                    {
                        MonsterManager.Instance.UsePool(monsterSpawnInfo[j].MonsterPoolIndex);
                    }
                    break;

                case WaveType.Boss:
                case WaveType.MiniBoss:

                    BossMonsterSpawnInfo[] _monsterInfo = nowStageData.StageWaves[i].ImmediateSpawnMonsters;

                    for (int j = 0; j < _monsterInfo.Length; ++j)
                    {
                        MonsterManager.Instance.UsePool(_monsterInfo[j].MonsterIndex);
                    }
                    break;
            }
        }

        // box 도 미리 로드
        MonsterManager.Instance.UsePool(MonsterPoolIndex.ItemBox);
 
        // 첫 웨이브 진행
        EnterWave(_waveQueue.Dequeue());
    }
    #endregion


    public void EnterWave(StageWaveEntry wave)
    {
        if (_nowWave != null)
        {
            // 이전 웨이브 클리어 보상 저장
            _saveExp += _nowWave.WaveClearExp;
            PlayerManager.Instance.StagePlayer.AddGold(_nowWave.WaveClearGold);

            SpawnWaveRewardBox();


            if (_readyWarnningSign == false &&
                (wave.WaveType == WaveType.Super || wave.WaveType == WaveType.Boss))
            {
                _readyWarnningSign = true;
            }

        }
        else
        {
            _readyWarnningSign = true;
        }



        _nowWave = wave;

        switch (wave.WaveType)
        {
            case WaveType.Continuous:
            case WaveType.Super:
                _nowContinuousWave = wave;

                _continuousSpawnDelays.Clear();
                for (int i = 0; i < _nowContinuousWave.ContinuouseMOnsterSpawnInfos.Length; ++i)
                {
                    _continuousSpawnDelays.Add(0);
                }

                break;

            case WaveType.MiniBoss:
                // 상시스폰은 유지하고
                // 특정 시간에 특정 몬스터 스폰
                for(int i = 0; i < _nowWave.ImmediateSpawnMonsters.Length ;++i)
                {
                    MonsterManager.Instance.SpawnWaveMonster(_nowWave.ImmediateSpawnMonsters[i].MonsterIndex);
                }
                break;
        }

    }


    #region 업데이트

    public void Update()
    {
        // 플레이 시간 갱신
        if (_nowWave.WaveType != WaveType.Boss)
        {
            _playTime += Time.deltaTime;

            // 상시 스폰 웨이브일 때, 스폰 시간 갱신
            UpdateSpawnMonster();

            // 아이템 박스 시간마다 스폰하기
            UpdateSpawnItemBox();

            // 다음 웨이브 시간 확인
            if (_waveQueue.Count > 0)
            {
                StageWaveEntry nextEntry = _waveQueue.Peek();
                UpdateWarnningSign(nextEntry);

                // 다음 웨이브 진행 확인
                if (nextEntry.WaveStartTime <= _playTime)
                {
                    StageWaveEntry nextWaveData = _waveQueue.Dequeue();
                    EnterWave(nextWaveData);
                }
            }

        }
        else    // 보스 웨이브일 때,
        {
            // 시간 맞춰서 보스몬스터 스폰하기 (보스웨이브 후 다른 웨이브 없이 n초 후 보스웨이브일 때)
            // 이미 스폰 되었다면 하지 않기
            if (_nowBossMonsters.Count <= 0)
            {
                if (_nowWave.WaveStartTime <= _playTime)
                {
                    SpawnBossMonster(_nowWave.ImmediateSpawnMonsters);
                }
                else
                {
                    _playTime += Time.deltaTime;
                    UpdateWarnningSign(_nowWave);
                }
            }

        }

    }


    void UpdateSpawnItemBox()
    {
        if (_playTime >= _itemBoxSpawnStartTime + (_itemBoxSpawnInterval * _itemBoxSpawnCount))
        {
            MonsterManager.Instance.SpawnWaveMonster(MonsterPoolIndex.ItemBox);
            _itemBoxSpawnCount++;
        }
    }

    void UpdateSpawnMonster()
    {
        if (_nowContinuousWave == null) return;
        if (_nowContinuousWave.ContinuouseMOnsterSpawnInfos == null) return;
        if (_nowContinuousWave.ContinuouseMOnsterSpawnInfos.Length == 0) return;

        if (_nowContinuousWave != null)
        {
            MonsterSpawnInfo[] spawnInfos = _nowContinuousWave.ContinuouseMOnsterSpawnInfos;

            for (int i = 0; i < spawnInfos.Length; ++i)
            {
                _continuousSpawnDelays[i] += Time.deltaTime;

                if (_continuousSpawnDelays[i] >= spawnInfos[i].SpawnDelay)
                {
                    for (int j = 0; j < spawnInfos[i].SpawnCount; ++j)
                    {
                        MonsterManager.Instance.SpawnWaveMonster(spawnInfos[i].MonsterPoolIndex);
                    }

                    _continuousSpawnDelays[i] = 0;
                }

            }

        }
    }

    void UpdateWarnningSign(StageWaveEntry targetWave)
    {
        if (targetWave.WaveType == WaveType.Boss ||
            targetWave.WaveType == WaveType.Super)
        {
            if (targetWave.WaveStartTime - 3f <= _playTime && _readyWarnningSign)
            {
                _readyWarnningSign = false;

                WarnningSignUI warnning = (WarnningSignUI)UIManager.Instance.ShowUI(UIName.UI_WarnningSign);
                switch (targetWave.WaveType)
                {
                    case WaveType.Super:
                        if(Camera.main.orthographicSize < _superWaveOrthoSize)
                        {
                            Camera.main.DOOrthoSize(_superWaveOrthoSize, 4.0f);
                        }

                        warnning.SetText(_superWaveWarnningSignText);
                        break;

                    case WaveType.Boss:
                        warnning.SetText(_bossWarnningSignText);
                        break;
                }
            }

        }
    }


    #endregion


    void SpawnWaveRewardBox()
    {
        // 이전 웨이브 보상 뿌리기
        //for (int i = 0; i < _nowWave.ClearRewardTypes.Length; ++i)
        //{

        //}
    }

    void SpawnBossMonster(BossMonsterSpawnInfo[] spawnInfo)
    {
        for (int i = 0; i < spawnInfo.Length; ++i)
        {
            Monster spawnedMonster = MonsterManager.Instance.SpawnBossMonster(spawnInfo[i].MonsterIndex);

            int num = i;

            WaveClearRewardType[] reward = spawnInfo[i].Rewards;

            Action<Monster> dieAction = null;

            dieAction = (Monster monster) =>
            {
                Debug.Log(monster.name + "_" + num + "_");
                monster.onDieAction -= dieAction;
            };

            spawnedMonster.onDieAction += dieAction;


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

                if (_nowWave != null)
                {
                    _saveExp += _nowWave.WaveClearExp;
                    PlayerManager.Instance.StagePlayer.AddGold(_nowWave.WaveClearGold);
                }

                OnStageEndAction?.Invoke();
            }
        }

        monster.onDieAction -= OnDieBossMonster;
    }

    
    void SpawnWaveClearReward(WaveClearRewardType[] rewards)
    {

    }
    

}