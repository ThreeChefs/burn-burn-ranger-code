using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage Wave Data", menuName = "SO/Stage Wave Data")]
public class StageWaveData : ScriptableObject
{
    [SerializeField] WaveType _waveType;
    public WaveType WaveType => _waveType;
    
    [SerializeField] private List<MonsterTypeData> _monsterTypeData;
    public List<MonsterTypeData> MonsterTypeData => _monsterTypeData;
    
    [SerializeField] private float _waveStartTime = 0f;
    public float WaveStartTime => _waveStartTime;

    [SerializeField] private float _spawnDelay = 1.0f;
    public float SpawnDelay => _spawnDelay;
    
    [SerializeField] private int _spawnCount = 1;
    public float SpawnCount => _spawnCount;
        
    [SerializeField] private WaveSpawnPosition _waveSpawnPosition;
    public WaveSpawnPosition WaveSpawnPosition => _waveSpawnPosition;
}

public enum WaveType
{
    Continuous,     // 지속적으로 나오는 타입. Super나 Boss wave가 시작될 때 종료
    Super,          // 몬스터가 몰려옵니다. 
    Named,          // 네임드 몬스터. WaveType 으로 따지면 한 번만 나올 타입
    Boss,
}

public enum WaveSpawnPosition
{
    Random,             // 지속적으로 나오는 타입일 경우 활용해야함.
    MapTopLeft,         // 맵 왼쪽 위 (북서쪽)
    MapTopCenter,
    MapTopRight,
    MapBottomLeft,
    MapBottomCenter,
    MapBottomRight,
    MapRight,
    MapLeft,
    // PlayerLeft,      // 플레이어의 바로 왼쪽.. 같은 느낌으로 해놨는데 필요한가..?        
    // PlayerRight,
    // PlayerTop,
    // PlayerBottom,
}