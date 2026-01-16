using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GlobalSingletonManager<GameManager>
{
    // 각종 매니저들
    public SceneController Scene = new();
    public DataManager Data { get; private set; }


    // GameManager가 들고 있을 플레이 정보들 (플레이어 정보 외)
    public StageProgress StageClearProgress = new();


    [SerializeField] SoDatabase _stageDatabase;
    public List<StageData> StageDatabase { get; private set; }


    protected override void Init()
    {
        StageDatabase = _stageDatabase.GetDatabase<StageData>();
        Data = new DataManager();
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



}