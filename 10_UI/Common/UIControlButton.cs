using System;
using UnityEngine;

public class UIControlButton : BaseButton
{
    [SerializeField] private string[] _showUIKeys;
    [SerializeField] private string[] _closeKeys;

    private UIName[] _showUIs;
    private UIName[] _closeUIs;

    protected override void Awake()
    {
        base.Awake();

        _showUIs = new UIName[_showUIKeys.Length];

        for (int i = 0; i < _showUIKeys.Length; i++)
        {
            if (!Enum.TryParse(_showUIKeys[i], out UIName ui))
            {
                Debug.LogError(
                    $"[UIControlButton] Invalid UIName key: {_showUIKeys[i]}",
                    this
                );
                continue;
            }

            _showUIs[i] = ui;
        }

        _closeUIs = new UIName[_closeKeys.Length];

        for (int i = 0; i < _closeKeys.Length; i++)
        {
            if (!Enum.TryParse(_closeKeys[i], out UIName ui))
            {
                Debug.LogError(
                    $"[UIControlButton] Invalid UIName key: {_closeKeys[i]}",
                    this
                );
                continue;
            }

            _closeUIs[i] = ui;
        }
    }

    protected override void OnClick()
    {
        base.OnClick();

        foreach (UIName uIName in _showUIs)
        {
            UIManager.Instance.ShowUI(uIName);
        }

        foreach (UIName uIName in _closeUIs)
        {
            UIManager.Instance.CloseUI(uIName);
        }
    }
}
