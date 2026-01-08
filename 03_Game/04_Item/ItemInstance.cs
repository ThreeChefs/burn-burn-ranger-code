/// <summary>
/// 보유하고 있는 실제 아이템 데이터
/// </summary>
public class ItemInstance
{
    public ItemClass ItemClass { get; private set; }

    public ItemData ItemData { get; private set; }

    public ItemInstance(ItemClass itemClass, ItemData itemData)
    {
        ItemClass = itemClass;
        ItemData = itemData;
    }

    public override string ToString()
    {
        return $"ItemClass: {ItemClass} " + ItemData.ToString();
    }
}
