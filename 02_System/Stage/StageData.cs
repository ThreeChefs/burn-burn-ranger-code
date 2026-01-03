using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage Data")]
public class StageData : ScriptableObject
{
    
    [SerializeField] List<StageWaveData> _monsterWaves = new List<StageWaveData>();
    
    private void OnValidate()
    {
        _monsterWaves.Sort((a, b) =>
            a.WaveStartTime.CompareTo(b.WaveStartTime)
        );
    }
}