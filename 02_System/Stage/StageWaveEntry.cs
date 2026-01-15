using System;
using UnityEngine;

[Serializable]
public class StageWaveEntry
{
    [Delayed]
    [SerializeField] private float _waveStartTime;
    public float WaveStartTime => _waveStartTime;

    [SerializeField] int _waveClearExp = 10;
    [SerializeField] int _waveClearGold = 100;
    [SerializeField] StageWaveData _stageWaveData;

    public StageWaveData WaveData => _stageWaveData;
    public int WaveClearExp => _waveClearExp;
    public int WaveClearGold => _waveClearGold;
}