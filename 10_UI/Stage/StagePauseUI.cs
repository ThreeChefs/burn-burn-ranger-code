using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class StagePauseUI : PopupUI
{
    [Title("StagePause UI")]
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _settingButton;

    private RectTransform _backRect;        // 애니메이션을 공통적으로 
    private RectTransform _homeRect;
    private RectTransform _settingRect;

    private Vector2 _backOriginPos;
    private Vector2 _homeOriginPos;
    private Vector2 _settingOriginPos;      // todo : Tween 만 하는 스크립트를 추가하기

    private void Awake()
    {
        UIManager.Instance.LoadUI(UIName.UI_Settings,false); // 이 UI에서 추가로 쓸거니까

        _backButton.onClick.AddListener(OnClickBackButton);
        _homeButton.onClick.AddListener(OnClickHomeButton);
        _settingButton.onClick.AddListener(OnClickSettingButton);

        _backRect = _backButton.GetComponent<RectTransform>();
        _homeRect = _homeButton.GetComponent<RectTransform>();
        _settingRect = _settingButton.GetComponent<RectTransform>();

        _backOriginPos = _backRect.anchoredPosition;
        _homeOriginPos = _homeRect.anchoredPosition;
        _settingOriginPos = _settingRect.anchoredPosition;
    }

    public override void OpenUIInternal()
    {
        base.OpenUIInternal();

        _backRect.DOKill();
        _homeRect.DOKill();
        _settingRect.DOKill();

        float backMove = _backRect.rect.width;
        float homeMove = _homeRect.rect.width;
        float settingMove = _settingRect.rect.width;

        _backRect.anchoredPosition = _backOriginPos + Vector2.left * backMove;
        _homeRect.anchoredPosition = _homeOriginPos + Vector2.left * homeMove;
        _settingRect.anchoredPosition = _settingOriginPos + Vector2.left * settingMove;

        _backRect.DOAnchorPos(_backOriginPos, PopupDuration).SetEase(Ease.OutCubic).SetUpdate(true);
        _homeRect.DOAnchorPos(_homeOriginPos, PopupDuration).SetEase(Ease.OutCubic).SetUpdate(true);
        _settingRect.DOAnchorPos(_settingOriginPos, PopupDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public override Tween CloseUIInternal()
    {
        _backRect.DOKill();
        _homeRect.DOKill();
        _settingRect.DOKill();

        float backMove = _backRect.rect.width;
        float homeMove = _homeRect.rect.width;
        float settingMove = _settingRect.rect.width;

        _backRect.DOAnchorPos(_backOriginPos + Vector2.left * backMove, PopupDuration)
            .SetEase(Ease.InCubic).SetUpdate(true);

        _homeRect.DOAnchorPos(_homeOriginPos + Vector2.left * homeMove, PopupDuration)
            .SetEase(Ease.InCubic).SetUpdate(true);

        _settingRect.DOAnchorPos(_settingOriginPos + Vector2.left * settingMove, PopupDuration)
            .SetEase(Ease.InCubic).SetUpdate(true);

        return base.CloseUIInternal();
    }

    void OnClickBackButton()
    {
        UIManager.Instance.CloseUI(UIName.UI_StagePause);
        StageManager.Instance.ResumeGame();
    }

    void OnClickHomeButton()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }

    void OnClickSettingButton()
    {
        UIManager.Instance.ShowUI(UIName.UI_Settings);
    }
}
