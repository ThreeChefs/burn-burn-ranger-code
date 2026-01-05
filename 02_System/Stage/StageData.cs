using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage/Stage Data")]
public class StageData : ScriptableObject
{
    [Title("Reward Data")]
    [SerializeField] private int _rewardExp = 100;      // 보상 경험치
    [SerializeField] private int _rewardGold = 100;     // 보상 골드
    [SerializeField] private int _rewardBoxCount = 5;   // 랜덤으로 장비 업그레이드 스크롤 나오는 갯수
    
    [Title("Map Data")]
    // todo 맵 어떻게 넣을지 고민하기
    
    [Title("Wave Data")]
    [SerializeField] List<StageWaveEntry> _stageWaves = new List<StageWaveEntry>();

    
    
    
    public List<StageWaveEntry> StageWaves => _stageWaves;
    
    public int RewardExp => _rewardExp;
    public int RewardGold => _rewardGold;
    public int RewardBoxCount => _rewardBoxCount;


    private void OnValidate()
    {      
        _stageWaves.Sort((a, b) =>
            a.WaveStartTime.CompareTo(b.WaveStartTime)
        );
    }
}