using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : GlobalSingletonManager<UIManager>
{
    [SerializeField] GoDatabase _uiDatabase;
    Dictionary<UIName, BaseUI> _originUiDict = new Dictionary<UIName, BaseUI>();
    Dictionary<UIName, BaseUI> _nowLoadedUiDict = new Dictionary<UIName, BaseUI>();


    [SerializeField] private Canvas _originCanvasPrefab;
    Canvas _mainCanvas;
    Dictionary<UISubCanvasOrder, Canvas> _subCanvasDict;


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

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainCanvas = Instantiate(_originCanvasPrefab);
        _mainCanvas.name = "MainCanvas";

        _subCanvasDict = new Dictionary<UISubCanvasOrder, Canvas>();

        foreach (UISubCanvasOrder e in Enum.GetValues(typeof(UISubCanvasOrder)))
        {
            Canvas newSubCanvas = Instantiate(_originCanvasPrefab);
            newSubCanvas.sortingOrder = (int)e;
            newSubCanvas.name = e + "Canvas";

            _subCanvasDict.Add(e, newSubCanvas);
        }



    }


    /// <summary>
    /// 1회성
    /// </summary>
    public BaseUI SpawnUI(UIName uiName)
    {
        BaseUI ui = GetOriginUI(uiName);

        if (ui != null)
        {
            BaseUI spawnedUI = Instantiate(ui);

            if (spawnedUI.IsSubCanvas == false)
            {
                spawnedUI.transform.SetParent(_mainCanvas.transform, false);
            }
            else
            {
                if (_subCanvasDict.ContainsKey(spawnedUI.SubUIOrder))
                {
                    spawnedUI.transform.SetParent(_subCanvasDict[spawnedUI.SubUIOrder].transform);
                }
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
    /// 현재 씬에서 Dictionary에 저장해두고 쓸 애들
    /// </summary>
    public BaseUI LoadUI(UIName uiName, bool active = true)
    {
        BaseUI ui = GetOriginUI(uiName);

        if (ui != null)
        {
            BaseUI spawnedUI = Instantiate(ui);

            if (spawnedUI.IsSubCanvas == false)
            {
                spawnedUI.transform.SetParent(_mainCanvas.transform, false);
            }
            else
            {
                if (_subCanvasDict.ContainsKey(spawnedUI.SubUIOrder))
                {
                    spawnedUI.transform.SetParent(_subCanvasDict[spawnedUI.SubUIOrder].transform, false);
                }
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


    public BaseUI ShowUI(UIName uiName)
    {
        BaseUI ui = GetNowSpawnedUI(uiName);
        if (ui != null)
        {
            // todo 가장 아래로 내리기
            ui.gameObject.SetActive(true);
            ui.OpenUI();
            ui.transform.SetAsLastSibling();
            return ui;
        }

        return null;
    }

    /// <summary>
    /// Load 되어있는 UI 중 골라서 닫기
    /// </summary>
    public BaseUI CloseUI(UIName uiName)
    {
        BaseUI ui = GetNowSpawnedUI(uiName);
        if (ui != null)
        {
            ui.CloseUI();
            return ui;
        }
        return null;
    }


    #region GetUI

    BaseUI GetOriginUI(UIName uiName)
    {
        if (_originUiDict.ContainsKey(uiName))
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