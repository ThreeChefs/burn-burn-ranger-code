using System;
using UnityEngine;

[Serializable]
public class StageWaveEntry
{
    [Delayed]
    [SerializeField] private float _waveStartTime;
    public float WaveStartTime => _waveStartTime;
    
    
    [SerializeField] StageWaveData _stageWaveData;
    public StageWaveData StageWaveData => _stageWaveData;
    
}