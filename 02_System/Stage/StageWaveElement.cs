using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

[Serializable]
public class MonsterSpawnInfo
{
    [SerializeField] MonsterPoolIndex _monsterIndex;
    [SerializeField] float _spawnDelay;
    [SerializeField] int _spawnCount;


    public MonsterPoolIndex MonsterPoolIndex => _monsterIndex;
    public int SpawnCount => _spawnCount;
    public float SpawnDelay => _spawnDelay;
}

[Serializable]
public class BossMonsterSpawnInfo
{
    [SerializeField] MonsterPoolIndex _monsterIndex;

    [LabelText("처치 보상")]
    [SerializeField] WaveClearRewardType[] _rewards;

    public MonsterPoolIndex MonsterIndex => _monsterIndex;
    public WaveClearRewardType[] Rewards => _rewards;
}


public enum WaveType
{
    Continuous,     // 지속적으로 나오는 타입. Super나 Boss wave가 시작될 때 종료
    Super,          // 몬스터가 몰려옵니다. Continuous 처럼 상시 웨이브가 됨.
    MiniBoss,       // 네임드 몬스터. WaveType 으로 따지면 한 번만 나올 타입
    Boss,           // 보스 몬스터. 기존에 있던 모든 몹들이 사라지고 보스만 나옴.
}

public enum WaveClearRewardType
{
    None = 0,

    Gold_Coin = 1,
    Gold_Ingot = 2,
    Gold_Pocket = 3,
    Gold_Box = 4,

    Item_Magnetic = 100,
    Item_Meat = 101,
    
    Fortune_Skill_Random = 200,
    Fortune_Skill_1 =201,
    Fortune_Skill_3 = 202,
    Fortune_Skill_5 = 203,
}
