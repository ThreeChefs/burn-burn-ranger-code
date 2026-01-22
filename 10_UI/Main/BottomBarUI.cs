using System;
using UnityEngine;

public class BottomBarUI : BaseUI
{
    [Header("버튼")]
    [SerializeField] private HomeBottomButton[] _buttons;

    private int _index;
    private const int StartIndex = 2;

    private const int OriginWidth = 200;
    private const int TargetWidth = 280;

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

        SelectButton(StartIndex);
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

            _buttons[i].MoveTo(x + OriginWidth / 2);
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _buttons = GetComponentsInChildren<HomeBottomButton>();
    }
#endif
}

