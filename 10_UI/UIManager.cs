using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : GlobalSingletonManager<UIManager>
{
    [SerializeField] GoDatabase _uiDatabase;
    Dictionary<UIPanelType,BaseUI> _originUiDict = new Dictionary<UIPanelType, BaseUI>();
    Dictionary<UIPanelType, BaseUI> _nowSpawnUiDict = new Dictionary<UIPanelType, BaseUI>();

    
    [SerializeField] private Canvas _originCanvasPrefab;
    Canvas _mainCanvas;
    
    protected override void Init()
    {
        _originUiDict = ((UIPanelType[])Enum.GetValues(typeof(UIPanelType))).
            ToDictionary(part => part, part => (BaseUI)null);
        
        
        List<BaseUI> _uiList = _uiDatabase.GetDatabaseComponent<BaseUI>();
        
        for (int i = 0; i < _uiList.Count; i++)
        {
            if (Enum.TryParse(_uiList[i].name, out UIPanelType uiName))
            {
                _originUiDict[uiName] = _uiList[i];
            }
        }
    }


    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    
    /// <summary>
    /// Scene 구성할 때 필요한 UI 부르기
    /// 팝업이나 일회용 UI 들은 그냥 스폰하기
    /// </summary>
    /// <param name="uiPanelType"></param>
    /// <returns></returns>
    public BaseUI SpawnUI(UIPanelType uiPanelType)
    {
        BaseUI ui = GetOriginUI(uiPanelType);
        
        if (ui != null)
        {
            return Instantiate(ui);
        }

        return null;
    }

    public BaseUI ShowUI(UIPanelType uiPanelType)
    {
        BaseUI ui = GetNowSpawnedUI(uiPanelType);
        if (ui != null)
        {
            // 가장 아래로 내리기
            return ui;
        }

        return null;
    }
    

    #region GetUI
    
    public BaseUI GetOriginUI(UIPanelType uiPanelType)
    {
        if(_originUiDict.ContainsKey(uiPanelType))
            return _originUiDict[uiPanelType];
        
        return null;
    }
    
    public BaseUI GetNowSpawnedUI(UIPanelType uiPanelType)
    {
        if (_nowSpawnUiDict.ContainsKey(uiPanelType))
            return _nowSpawnUiDict[uiPanelType];
        return null;
    }

    #endregion


    

    
    
    
    
    protected override void OnSceneUnloaded(Scene scene)
    {
        _nowSpawnUiDict.Clear();  
    }
}




public enum UIPanelType
{
    UI_StageSelect,
    
}