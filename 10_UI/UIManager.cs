using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : GlobalSingletonManager<UIManager>
{
    [SerializeField] GoDatabase _uiDatabase;   // UI 원본 프리팹들이 저장된 데이터베이스

    // UIName 과 BaseUI 원본 프리팹 매핑 딕셔너리
    Dictionary<UIName, BaseUI> _originUiDict = new Dictionary<UIName, BaseUI>();
    
    // 현재 씬에서 로드된 UI 들을 저장하는 딕셔너리
    Dictionary<UIName, BaseUI> _nowLoadedUiDict = new Dictionary<UIName, BaseUI>();

    // 캔버스 원본 프리팹
    [SerializeField] private Canvas _originCanvasPrefab;
    Dictionary<UICanvasOrder, Canvas> _canvasDict;  // 각 오더별 캔버스 매핑 딕셔너리

    GameObject _canvasRoot;

    // UIManager 초기화
    // Dictionary 에 UIName 과 BaseUI 원본 프리팹 매핑
    protected override void Init()
    {
        _originUiDict = ((UIName[])Enum.GetValues(typeof(UIName))).ToDictionary(part => part, part => (BaseUI)null);

        List<BaseUI> _uiList = _uiDatabase.GetDatabaseComponent<BaseUI>();

        for (int i = 0; i < _uiList.Count; i++)
        {
            if (Enum.TryParse(_uiList[i].name, out UIName uiName))
            {
                _originUiDict[uiName] = _uiList[i];
            }
        }
    }

    // 씬 로드 시 처리
    // 캔버스 루트 오브젝트 및 각 오더별 캔버스 생성
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _canvasRoot = new GameObject();
        _canvasRoot.name = "CanvasRoot";
        _canvasRoot.transform.position = Vector3.zero;

        _canvasDict = new Dictionary<UICanvasOrder, Canvas>();

        foreach (UICanvasOrder e in Enum.GetValues(typeof(UICanvasOrder)))
        {
            Canvas newCanvas = Instantiate(_originCanvasPrefab);
            newCanvas.sortingOrder = ((int)e);
            newCanvas.name = e + "Canvas";

            _canvasDict.Add(e, newCanvas);

            newCanvas.transform.SetParent(_canvasRoot.transform, false);
        }

        // 항상 스폰해둘 UI
        LoadUI(UIName.UI_Dimmed, false);
    }


    /// <summary>
    /// 1회성 UI 스폰
    /// </summary>
    public BaseUI SpawnUI(UIName uiName)
    {
        BaseUI ui = GetOriginUI(uiName);

        if (ui != null)
        {
            BaseUI spawnedUI = Instantiate(ui);


            if (_canvasDict.ContainsKey(spawnedUI.UIOrder))
            {
                spawnedUI.transform.SetParent(_canvasDict[spawnedUI.UIOrder].transform, false);
            }


            spawnedUI.OpenUI();

            RectTransform rect = spawnedUI.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }


            return spawnedUI;
        }

        return null;
    }

    /// <summary>
    /// 월드 UI 스폰
    /// </summary>
    public BaseUI SpawnWorldUI(UIName uiName, Transform parent = null)
    {
        BaseUI ui = GetOriginUI(uiName);

        if (ui != null)
        {
            BaseUI spawnedUI = Instantiate(ui, parent == null ? null : parent.transform, false);
            return spawnedUI;
        }

        return null;
    }

    /// <summary>
    /// 현재 씬에서 Dictionary에 저장해두고 쓸 UI 스폰
    /// </summary>
    public BaseUI LoadUI(UIName uiName, bool active = true)
    {
        BaseUI ui = GetOriginUI(uiName);

        if (ui != null)
        {
            BaseUI spawnedUI = Instantiate(ui);
            
            if (_canvasDict.ContainsKey(spawnedUI.UIOrder))
            {
                spawnedUI.transform.SetParent(_canvasDict[spawnedUI.UIOrder].transform, false);
            }

            RectTransform rect = spawnedUI.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }

            if (active == false)
                spawnedUI.gameObject.SetActive(false);
            else
                spawnedUI.OpenUI();

            if (_nowLoadedUiDict.ContainsKey(uiName) == false)
            {
                _nowLoadedUiDict.Add(uiName, spawnedUI);
            }

            return spawnedUI;
        }

        return null;
    }


    /// <summary>
    /// Load 되어있는 UI 중 골라서 열기
    /// </summary>
    public BaseUI ShowUI(UIName uiName)
    {
        BaseUI ui = GetLoadedUI(uiName);
        if (ui != null)
        {
            ui.transform.SetAsLastSibling();    // UI 현재 부모 안에서 최상단으로 올리기
            ui.gameObject.SetActive(true);
            ui.OpenUI();
            return ui;
        }

        return null;
    }

    /// <summary>
    /// Load 되어있는 UI 중 골라서 닫기
    /// </summary>
    public BaseUI CloseUI(UIName uiName)
    {
        BaseUI ui = GetLoadedUI(uiName);
        if (ui != null)
        {
            ui.CloseUI();
            return ui;
        }
        return null;
    }

    // UIManager 를 통해 UI를 가져오는 함수들
    #region GetUI

    BaseUI GetOriginUI(UIName uiName)
    {
        if (_originUiDict.ContainsKey(uiName))
            return _originUiDict[uiName];

        return null;
    }

    public BaseUI GetLoadedUI(UIName uiName)
    {
        if (_nowLoadedUiDict.ContainsKey(uiName))
            return _nowLoadedUiDict[uiName];
        return null;
    }

    #endregion

    /// <summary>
    /// 씬 언로드 시 처리
    /// </summary>
    protected override void OnSceneUnloaded(Scene scene)
    {
        _canvasRoot = null;
        _nowLoadedUiDict.Clear();
    }
}