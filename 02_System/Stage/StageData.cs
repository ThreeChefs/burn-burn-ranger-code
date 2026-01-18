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
    [DetailedInfoBox(
        "웨이브 설정 설명",
        "[WaveType]\n" +
        "＊Continuous : 상시 스폰\n" +
        "＊Super : 상시스폰과 같은 동작. 몰려옴 알림울림\n" +
        "＊Miniboss : 해당 시간에 특정 몬스터 등장. 다음 상시 스폰까지는 이전 상시스폰 지속됨.\n" +
        "＊Boss : 보스 등장. 스폰된 일반 몬스터 사라짐."
         )]
    [SerializeField] List<StageWaveEntry> _stageWaves = new List<StageWaveEntry>();


    // public 프로퍼티
    public string StageName => _stageName;
    public Sprite StageIcon => _stageIcon;

    public int RewardExp => _rewardExp;
    public int RewardGold => _rewardGold;
    public int RewardBoxCount => _rewardBoxCount;
    public ItemBoxData ItemBoxData => _itemBoxData;

    public InfiniteMap Map => _map;

    public List<StageWaveEntry> StageWaves => _stageWaves;



    // SO Inspector

    [PropertyOrder(-100)]
    [Button("기본 웨이브 생성")]
    void InitWaveData()
    {

    }

    private void OnValidate()
    {
        _stageWaves.Sort((a, b) =>
            a.WaveStartTime.CompareTo(b.WaveStartTime)
        );
    }
}