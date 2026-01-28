using System;
using UnityEngine;

public class BottomBarUI : BaseUI
{
    [Header("버튼")]
    [SerializeField] private RectTransform _parent;
    [SerializeField] private HomeBottomButton[] _buttons;

    [Header("설정")]
    [SerializeField] private int _startIndex = 2;
    [SerializeField] private float _maxButtonWidth = 300f;
    private int _index;

    private float OriginWidth => Mathf.Min(_parent.rect.width * 1 / (_buttons.Length - 1 + Define.TargetScale), _maxButtonWidth);
    private float TotalWidth => OriginWidth * (_buttons.Length - 1) + TargetWidth;
    private float TargetWidth => OriginWidth * Define.TargetScale;

    public event Action<BottomBarMenuType> OnClickMenuAction;

    protected override void AwakeInternal()
    {
        base.AwakeInternal();
        _index = -1;
    }

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].Init(i);
            _buttons[i].OnClickButton += SelectButton;
        }

        SelectButton(_startIndex);
    }

    private void OnEnable()
    {
        foreach (HomeBottomButton button in _buttons)
        {
            button.OnClickButton += SelectButton;
        }
    }

    private void OnDisable()
    {
        foreach (HomeBottomButton button in _buttons)
        {
            button.OnClickButton -= SelectButton;
        }
    }

    private void SelectButton(int index)
    {
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Click);
        OnClickMenuAction?.Invoke((BottomBarMenuType)index);

        float center = _parent.rect.width * 0.5f;
        float startPos = center - TotalWidth / 2;

        if (_index == index) return;

        if (_index != -1)
        {
            _buttons[_index].SetSelected(false);
        }
        _buttons[index].SetSelected(true);

        _index = index;

        float offset = TargetWidth - OriginWidth;

        for (int i = 0; i < _buttons.Length; i++)
        {
            float x = OriginWidth * i;

            if (i == _index)
            {
                x += offset / 2;
            }
            else if (i > _index)
            {
                x += offset;
            }

            _buttons[i].SetWidth(OriginWidth);
            _buttons[i].MoveTo(x + startPos + OriginWidth / 2);
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _buttons = GetComponentsInChildren<HomeBottomButton>();
    }
#endif
}

