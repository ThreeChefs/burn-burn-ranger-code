using System.Collections.Generic;
using UnityEngine;

public class GameManager : GlobalSingletonManager<GameManager>
{
    // 각종 매니저들
    public SceneController Scene = new();
    public DataManager Data { get; private set; }

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


}