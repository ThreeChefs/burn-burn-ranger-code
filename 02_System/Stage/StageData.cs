using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage/Stage Data")]
public class StageData : ScriptableObject
{
    [Title("Stage Info")]
    [SerializeField] private string _stageName = "스테이지";
    [SerializeField] private Sprite _stageIcon;


    [Title("Reward Data")]
    [SerializeField] private int _rewardExp = 100;      // 보상 경험치
    [SerializeField] private int _rewardGold = 100;     // 보상 골드
    [SerializeField] private int _rewardBoxCount = 5;   // 랜덤으로 장비나 업그레이드 스크롤 나오는 갯수
    [SerializeField] ItemBoxData _itemBoxData;          // 랜덤으로 나올 장비의 정보들 (픽업시스템 재활용)
    
    [Title("Map Data")]
    [SerializeField] InfiniteMap _map;
    
    [Title("Wave Data")]
    [SerializeField] List<StageWaveEntry> _stageWaves = new List<StageWaveEntry>();
    
    
    public List<StageWaveEntry> StageWaves => _stageWaves;
    
    public InfiniteMap Map => _map;
    public int RewardExp => _rewardExp;
    public int RewardGold => _rewardGold;
    public int RewardBoxCount => _rewardBoxCount;
    public string StageName => _stageName;
    public Sprite StageIcon => _stageIcon;
    public ItemBoxData ItemBoxData => _itemBoxData;


    private void OnValidate()
    {      
        _stageWaves.Sort((a, b) =>
            a.WaveStartTime.CompareTo(b.WaveStartTime)
        );
    }
}