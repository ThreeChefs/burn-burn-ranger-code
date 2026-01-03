using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage/Stage Data")]
public class StageData : ScriptableObject
{
    public List<StageWaveEntry> StageWaves => _stageWaves;
    [SerializeField] List<StageWaveEntry> _stageWaves = new List<StageWaveEntry>();


    private void OnValidate()
    {      
        _stageWaves.Sort((a, b) =>
            a.WaveStartTime.CompareTo(b.WaveStartTime)
        );
    }
}