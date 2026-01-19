using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HomeBottomButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _backGround;
    [SerializeField] private RectTransform _icon;
    [SerializeField] private GameObject _textGo;

    [SerializeField] private UIName _targetUI;

    private int _index;
    public event Action<int> OnClickButton;

    private Sequence _seq;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        _seq?.Kill();
    }

    public void Init(int index)
    {
        _index = index;
    }

    private void OnClick()
    {
        UIManager.Instance.ShowUI(_targetUI);
        OnClickButton?.Invoke(_index);
    }

    public void SetSelected(bool selected)
    {
        PlayAnim(
            posY: selected ? Define.TargetPosY : Define.OriginPosY,
            scaleX: selected ? Define.TargetScale : Define.OriginScale,
            color: selected ? Define.TargetColor : Define.OriginColor,
            showText: selected
        );
    }

    private void PlayAnim(float posY, float scaleX, Color color, bool showText)
    {
        _seq?.Kill();
        _seq = DOTween.Sequence();

        _seq.Join(_icon.DOAnchorPosY(posY, Define.Duration))
            .Join(_backGround.transform.DOScaleX(scaleX, Define.Duration))
            .Join(_backGround.DOColor(color, Define.Duration))
            .OnStart(() => _textGo.SetActive(showText));
    }

    public void MoveTo(float targetX)
    {
        RectTransform rect = (RectTransform)transform;
        rect.DOKill();
        rect.DOAnchorPosX(targetX, Define.Duration);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _button = GetComponent<Button>();
        _backGround = transform.FindChild<Image>("Image - Bg");
        _icon = transform.FindChild<RectTransform>("Image - Icon");
        _textGo = transform.FindChild<TextMeshProUGUI>("Text (TMP)").gameObject;

        string uiEnumValue = "UI_" + ((name.Split("Button_Bottom_").Length > 1) ? name.Split("Button_Bottom_")[1] : "");
        if (!Enum.TryParse(uiEnumValue, out _targetUI))
        {
            Logger.LogWarning($"UIName 없음: {uiEnumValue}");
        }
    }
#endif
}
