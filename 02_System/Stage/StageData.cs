using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage/Stage Data")]
public class StageData : ScriptableObject
{
    public List<StageWaveData> MonsterWaves => _monsterWaves;
    [SerializeField] List<StageWaveData> _monsterWaves = new List<StageWaveData>();
    
    private void OnValidate()
    {
        _monsterWaves.Sort((a, b) =>
            a.WaveStartTime.CompareTo(b.WaveStartTime)
        );
    }
}