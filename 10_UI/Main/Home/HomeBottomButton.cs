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

    [Header("애니메이션")]
    [SerializeField] private float _originPosY = 0f;
    [SerializeField] private float _targetPosY = 50f;
    [SerializeField] private Color _originColor = new(0.20784315f, 0.20784315f, 0.3019608f);
    [SerializeField] private Color _targetColor = new(1f, 0.64705884f, 0.19607845f);
    [SerializeField] private float _originScale = 1f;
    [SerializeField] private float _targetScale = 1.4f;

    [SerializeField] private float _duration = 1f;

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
            posY: selected ? _targetPosY : _originPosY,
            scaleX: selected ? _targetScale : _originScale,
            color: selected ? _targetColor : _originColor,
            showText: selected
        );
    }

    private void PlayAnim(float posY, float scaleX, Color color, bool showText)
    {
        _seq?.Kill();
        _seq = DOTween.Sequence();

        _seq
            .Join(_icon.DOAnchorPosY(posY, _duration))
            .Join(_backGround.transform.DOScaleX(scaleX, _duration))
            .Join(_backGround.DOColor(color, _duration))
            .OnStart(() => _textGo.SetActive(showText));
    }


    public void MoveTo(float targetX)
    {
        RectTransform rect = (RectTransform)transform;
        rect.DOKill();
        rect.DOAnchorPosX(targetX, _duration);
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
