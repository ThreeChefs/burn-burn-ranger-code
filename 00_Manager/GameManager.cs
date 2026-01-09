using System;
using UnityEngine;

public class GameManager : GlobalSingletonManager<GameManager>
{
    // 각종 매니저들
    public SceneController Scene = new();
    public DataManager Data = new();

    [SerializeField] SoDatabase _stageDatabase;
    public SoDatabase StageDatabase => _stageDatabase;

    private void OnApplicationQuit()
    {
        Data.Save();
    }


    #region StageSelect
    
    private int _selectedStageNumber = 1;
    public  int SelectedStageNumber => _selectedStageNumber;

    public void SetSelectedStage(int stageNumber)
    {
        _selectedStageNumber = stageNumber;
    }

    #endregion
    
    
}