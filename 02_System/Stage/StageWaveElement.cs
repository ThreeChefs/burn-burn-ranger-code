using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class MonsterSpawnInfo
{
    [SerializeField] MonsterPoolIndex _monsterIndex;

    [TableColumnWidth(40)]
    [SerializeField] float _spawnDelay;

    [TableColumnWidth(40)]
    [SerializeField] int _spawnCount;


    public MonsterPoolIndex MonsterPoolIndex => _monsterIndex;
    public int SpawnCount => _spawnCount;
    public float SpawnDelay => _spawnDelay;
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
    Gold_Coin = 0,
    Gold_Ingot = 1,
    Gold_Pocket = 2,
    Gold_Box = 3,

    Item_Magnetic = 100,
    Item_Meat = 101,
    
    Fortune_Skill_Random = 200,
    Fortune_Skill_1 =201,
    Fortune_Skill_3 = 202,
    Fortune_Skill_5 = 203,
}
