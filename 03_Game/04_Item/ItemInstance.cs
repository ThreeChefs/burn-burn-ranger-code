/// <summary>
/// 보유하고 있는 실제 아이템 데이터
/// </summary>
public class ItemInstance
{
    public ItemClass ItemClass { get; private set; }
    public ItemData ItemData { get; private set; }
    public int Count { get; private set; }
    public int Level { get; private set; }

    /// <summary>
    /// [public] 생성자
    /// </summary>
    /// <param name="itemClass"></param>
    /// <param name="itemData"></param>
    public ItemInstance(ItemClass itemClass, ItemData itemData)
    {
        ItemClass = itemClass;
        ItemData = itemData;
        Count = 1;
        Level = 1;
    }

    public override string ToString()
    {
        return $"ItemClass: {ItemClass} " + ItemData.ToString();
    }
}
