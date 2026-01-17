using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 관리
/// </summary>
[System.Serializable]
public class SceneController
{
    #region Fields

    [SerializeField] private SceneDatabase _sceneDatabase;

    private Dictionary<SceneType, Type> _sceneTypeDict;

    private SceneType _curSceneType;
    private string _externalSceneName;

    private Coroutine _coroutine;
    private readonly WaitForSeconds _loadDelay = new(0.3f);
    #endregion

    #region 초기화
    public SceneController()
    {
        InitSceneTypeDict();
    }

    /// <summary>
    /// BaseScene의 Type을 캐싱해놓을 딕셔너리
    /// BaseScene에 Init이 있는 경우에만 추가하면 됩니다.
    /// </summary>
    private void InitSceneTypeDict()
    {
        _sceneTypeDict = new()
        {
        };
    }
    #endregion

    #region 씬 로드
    /// <summary>
    /// type과 동일한 이름의 씬을 비동기로 로드합니다.
    /// </summary>
    /// <param name="type"></param>
    public void LoadSceneWithCoroutine(SceneType type)
    {
        if (type == _curSceneType)
        {
            Logger.LogWarning("동일한 씬 로드");
            return;
        }
        _curSceneType = type;
        _externalSceneName = null;

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (_coroutine != null)
        {
            CustomCoroutineRunner.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = CustomCoroutineRunner.Instance.StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    /// sceneName과 동일한 이름의 씬을 비동기로 로드합니다.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneWithCoroutine(string sceneName)
    {
        if (sceneName == _curSceneType.ToString())
        {
            Logger.LogWarning("동일한 씬 로드");
            return;
        }
        _curSceneType = SceneType.None;
        _externalSceneName = sceneName;

        if (_coroutine != null)
        {
            CustomCoroutineRunner.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = CustomCoroutineRunner.Instance.StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    /// 현재 씬을 비동기 리로드 합니다.
    /// </summary>
    public void ReLoadSceneAsync()
    {
        if (_externalSceneName == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        if (_coroutine != null)
        {
            CustomCoroutineRunner.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = CustomCoroutineRunner.Instance.StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    /// 코루틴 비동기로 씬을 로딩합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadSceneAsync()
    {
        // todo: 로딩 씬 필요
        string sceneName = _curSceneType == SceneType.None ? _externalSceneName : _curSceneType.ToString();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(SceneType.LoadingScene.ToString()))
        {
            yield return LoadBootstrap(sceneName);
        }
        else
        {
            yield return LoadInGame(sceneName);
        }
    }

    private IEnumerator LoadBootstrap(string sceneName)
    {
        BootstrapLoadingUI loadingUI = UIManager.Instance.ShowUI(UIName.UI_BootstrapLoading) as BootstrapLoadingUI;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Logger.Log($"{_curSceneType}으로 로딩 중...");
        while (async.progress < 0.9f)
        {
            if (loadingUI != null) { loadingUI.SetProgress(async.progress); }
            Logger.Log($"진행률: {async.progress}");
            yield return null;
        }

        if (loadingUI != null) { loadingUI.SetProgress(1f); }

        yield return _loadDelay;
        async.allowSceneActivation = true;
    }

    private IEnumerator LoadInGame(string sceneName)
    {
        UIManager.Instance.ShowUI(UIName.UI_InGameLoading);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Logger.Log($"{_curSceneType}으로 로딩 중...");
        while (async.progress < 0.9f)
        {
            Logger.Log($"진행률: {async.progress}");
            yield return null;
        }

        async.allowSceneActivation = true;
    }

    /// <summary>
    /// 씬 로드가 완료된 직후 실행됩니다.
    /// 씬에 BaseScene이 필요한 경우 초기화합니다.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (!_sceneTypeDict.TryGetValue(_curSceneType, out Type type))
        {
            Logger.Log("base scene type 없음");
            return;
        }

        if (!_sceneDatabase.TryGetScene(_curSceneType, out GameObject prefab))
        {
            Logger.Log("scene database에 scene data 없음");
            return;
        }

        Logger.Log("scene prefab 생성");
        var sceneObj = (BaseScene)GameObject.Instantiate(prefab).GetComponent(type);
        sceneObj.Init();
    }
    #endregion
}
