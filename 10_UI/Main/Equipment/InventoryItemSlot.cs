/// <summary>
/// 인벤토리 버튼
/// </summary>
public class InventoryItemSlot : ItemSlot
{
    protected override void OnClickButton()
    {
        ItemDetailUI ui = UIManager.Instance.ShowUI(UIName.UI_ItemDetail) as ItemDetailUI;
        ui.SetItem(instance);
    }
}
