using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage Wave Data", menuName = "SO/Stage/Stage Wave Data")]
public class StageWaveData : ScriptableObject
{
    [SerializeField] WaveType _waveType;
    public WaveType WaveType => _waveType;

    [SerializeField] private List<MonsterPoolIndex> _monsters;
    public List<MonsterPoolIndex> Monsters => _monsters;

    [HideIf(nameof(_waveType), WaveType.Boss)]
    [SerializeField] private float _spawnDelay = 1.0f;
    public float SpawnDelay => _spawnDelay;

    [HideIf(nameof(_waveType), WaveType.Boss)]   // 보스는 list에 있는거 한 마리씩만
    [SerializeField] private int _spawnCount = 1;
    public float SpawnCount => _spawnCount;

    [SerializeField] private WaveClearRewardType[] _clearRewardType;
    public WaveClearRewardType[] ClearRewardType => _clearRewardType;

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
