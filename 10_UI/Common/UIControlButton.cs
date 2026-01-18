using UnityEngine;

public class UIControlButton : BaseButton
{
    [SerializeField] private UIName[] _showUIs;
    [SerializeField] private UIName[] _closeUIs;

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
