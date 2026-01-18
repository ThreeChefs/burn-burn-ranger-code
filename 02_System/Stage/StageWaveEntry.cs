using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class StageWaveEntry
{
    [GUIColor(1, 0.6f, 0.4f)]
    [Tooltip("웨이브 시작 시간")]
    [Delayed, SerializeField] private float _waveStartTime;

    [Tooltip("웨이브 종류")]
    [SerializeField] private WaveType _waveType;

    [FoldoutGroup("보상")]
    [FoldoutGroup("보상/결과 보상")]
    [Tooltip("웨이브 클리어 경험치 / 결과 확인 시 받음")]
    [SerializeField] int _waveClearExp = 10;

    [FoldoutGroup("보상/결과 보상")]
    [Tooltip("웨이브 클리어 골드 /  결과 확인 시 받음")]
    [SerializeField] int _waveClearGold = 100;

    [FoldoutGroup("보상/필드 보상")]
    [SerializeField] private WaveClearRewardType[] _clearRewardType;

    [FoldoutGroup("웨이브 등장 몬스터 설정")]
    [ShowIfGroup("웨이브 등장 몬스터 설정/@_isContinuousWave")]
    [TableList][SerializeField] private MonsterSpawnInfo[] _continuousMonsterSpawnInfos;

    [ShowIfGroup("웨이브 등장 몬스터 설정/@!_isContinuousWave")]
    [SerializeField] private MonsterPoolIndex[] _immediateSpawnMonsters;

    // public 프로퍼티
    public float WaveStartTime => _waveStartTime;

    public WaveType WaveType => _waveType; 

    public int WaveClearExp => _waveClearExp;

    public int WaveClearGold => _waveClearGold;

    public WaveClearRewardType[] ClearRewardTypes => _clearRewardType;

    public MonsterSpawnInfo[] ContinuouseMOnsterSpawnInfos => _continuousMonsterSpawnInfos;
    public MonsterPoolIndex[] ImmediateSpawnMonsters => _immediateSpawnMonsters;


    // Inspector 용 파라미터
    bool _isContinuousWave => _waveType == WaveType.Continuous || _waveType == WaveType.Super;

    public StageWaveEntry(
        float waveStartTime,
        WaveType waveType,
        int waveClearExp = 10,
        int waveClearGold = 100,
        WaveClearRewardType[] clearRewardTypes = null,
        MonsterSpawnInfo[] continuousMonsterSpawnInfos = null,
        MonsterPoolIndex[] immediateSpawnMonsters = null
    )
    {
        _waveStartTime = waveStartTime;
        _waveType = waveType;

        _waveClearExp = waveClearExp;
        _waveClearGold = waveClearGold;

        _clearRewardType = clearRewardTypes;

        _continuousMonsterSpawnInfos = continuousMonsterSpawnInfos;
        _immediateSpawnMonsters = immediateSpawnMonsters;
    }
}