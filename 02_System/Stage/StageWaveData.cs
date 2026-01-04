using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage Wave Data", menuName = "SO/Stage/Stage Wave Data")]
public class StageWaveData : ScriptableObject
{
    [SerializeField] WaveType _waveType;
    public WaveType WaveType => _waveType;
    
    [SerializeField] private List<MonsterTypeData> _monsterTypeData;
    public List<MonsterTypeData> MonsterTypeData => _monsterTypeData;
    
    [SerializeField] private float _spawnDelay = 1.0f;
    public float SpawnDelay => _spawnDelay;
    
    [HideIf(nameof(_waveType),WaveType.Boss)]   // 보스는 list에 있는거 한 마리씩만
    [SerializeField] private int _spawnCount = 1;
    public float SpawnCount => _spawnCount;
    
}

public enum WaveType
{
    Continuous,     // 지속적으로 나오는 타입. Super나 Boss wave가 시작될 때 종료
    Super,          // 몬스터가 몰려옵니다. 
    MiniBoss,          // 네임드 몬스터. WaveType 으로 따지면 한 번만 나올 타입
    Boss,
}
