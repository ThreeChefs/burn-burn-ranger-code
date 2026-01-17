using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GlobalSingletonManager<GameManager>
{
    // 각종 매니저들
    public SceneController Scene { get; private set; }
    public DataManager Data { get; private set; }


    // GameManager가 들고 있을 플레이 정보들 (플레이어 정보 외)
    public StageProgress StageClearProgress = new();
    public GrowthProgress GrowthProgress = new();


    [Header("데이터베이스")]
    [SerializeField] SoDatabase _stageDatabase;
    [SerializeField] private SoDatabase _itemBoxDatabase;
    [SerializeField] GrowthDatabase _growthDatabase;
    [SerializeField] private ItemDatabase _itemDatabase;
    public ItemDatabase ItemDatabase => _itemDatabase;

    public List<StageData> StageDatabase { get; private set; }
    public List<GrowthInfoEntry> GrowthInfoSetp => _growthDatabase.GrowInfoEntries;

    // 시스템
    public PickUpSystem PickUpSystem { get; private set; }

    protected override void Init()
    {
        Scene = new();
        StageDatabase = _stageDatabase.GetDatabase<StageData>();
        Data = new DataManager();

        PickUpSystem = new(_itemBoxDatabase);
    }

    private void OnApplicationQuit()
    {
        // Data.Save();
    }

    #region StageSelect

    private int _selectedStageNumber = 1;
    public int SelectedStageNumber => _selectedStageNumber;

    public void SetSelectedStage(int stageNumber)
    {
        _selectedStageNumber = stageNumber;
    }

    #endregion



    #region 저장

    [Title("저장 시스템 테스트")]
    [Button("저장")]
    public void SaveData()
    {

    }


    [Button("불러오기")]
    public void LoadData()
    {
    }


    #endregion

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale= 1.0f;
    }


#if UNITY_EDITOR
    private void Reset()
    {
        _stageDatabase = AssetLoader.FindAndLoadByName<SoDatabase>("StageDatabase");
        _itemBoxDatabase = AssetLoader.FindAndLoadByName<SoDatabase>("ItemBoxDatabase");
        _growthDatabase = AssetLoader.FindAndLoadByName<GrowthDatabase>("GrowthDatabase");
        _itemDatabase = AssetLoader.FindAndLoadByName<ItemDatabase>("ItemDatabase");
    }
#endif
}