/// <summary>
/// 버튼 - 아이템 합성
/// </summary>
public class ComposeButton : BaseButton
{
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    protected override void OnClick()
    {
        base.OnClick();
        UIManager.Instance.LoadUI(UIName.UI_ItemCompose);
    }
}