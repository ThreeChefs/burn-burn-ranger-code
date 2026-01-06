using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : GlobalSingletonManager<UIManager>
{
    [SerializeField] GoDatabase _uiDatabase;
    Dictionary<UIName,BaseUI> _originUiDict = new Dictionary<UIName, BaseUI>();
    Dictionary<UIName, BaseUI> _nowLoadedUiDict = new Dictionary<UIName, BaseUI>();

    
    [SerializeField] private Canvas _originCanvasPrefab;
    Canvas _mainCanvas;
    
    protected override void Init()
    {
        _originUiDict = ((UIName[])Enum.GetValues(typeof(UIName))).
            ToDictionary(part => part, part => (BaseUI)null);
        
        
        List<BaseUI> _uiList = _uiDatabase.GetDatabaseComponent<BaseUI>();
        
        for (int i = 0; i < _uiList.Count; i++)
        {
            if (Enum.TryParse(_uiList[i].name, out UIName uiName))
            {
                _originUiDict[uiName] = _uiList[i];
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnUI(UIName.UI_SkillSelect);
        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainCanvas = Instantiate(_originCanvasPrefab);

    }

    
    /// <summary>
    /// 1회성
    /// </summary>
    public BaseUI SpawnUI(UIName uiName, bool active = true)
    {
        BaseUI ui = GetOriginUI(uiName);
        
        if (ui != null)
        {   
            BaseUI spawnedUI = Instantiate(ui);
            spawnedUI.transform.SetParent(_mainCanvas.transform, false);
            
            if(active == false)
                spawnedUI.gameObject.SetActive(false);  // 프리팹이 켜져있으면 OnEnable 은 호출됨
            
            return spawnedUI;
        }

        return null;
    }
    
    /// <summary>
    /// 현재 씬에서 Dictionary에 저장해두고 쓸 애들
    /// </summary>
    public BaseUI LoadUI(UIName uiName, bool active = true)
    {
        BaseUI ui = GetOriginUI(uiName);
        
        if (ui != null)
        {   
            BaseUI spawnedUI = Instantiate(ui);
            spawnedUI.transform.SetParent(_mainCanvas.transform, false);
            
            if(active == false)
                spawnedUI.gameObject.SetActive(false);

            if (_nowLoadedUiDict.ContainsKey(uiName) == false)
            {
                _nowLoadedUiDict.Add(uiName, spawnedUI);
            }
            
            return spawnedUI;
        }

        return null;
    }



    public BaseUI ShowUI(UIName uiName)
    {
        BaseUI ui = GetNowSpawnedUI(uiName);
        if (ui != null)
        {
            // todo 가장 아래로 내리기
            return ui;
        }

        return null;
    }
    

    #region GetUI
    
    BaseUI GetOriginUI(UIName uiName)
    {
        if(_originUiDict.ContainsKey(uiName))
            return _originUiDict[uiName];
        
        return null;
    }
    
    public BaseUI GetNowSpawnedUI(UIName uiName)
    {
        if (_nowLoadedUiDict.ContainsKey(uiName))
            return _nowLoadedUiDict[uiName];
        return null;
    }

    #endregion


    

    
    
    
    
    protected override void OnSceneUnloaded(Scene scene)
    {
        _nowLoadedUiDict.Clear();  
    }
}




public enum UIName
{
    UI_StageSelect,
    UI_SkillSelect,
    UI_Stage,
}
