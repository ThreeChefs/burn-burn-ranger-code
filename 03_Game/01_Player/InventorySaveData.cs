using System.Collections.Generic;

public class InventorySaveData
{
    public List<ItemSaveData> List { get; private set; } = new();
}

public struct ItemSaveData
{
    public ItemClass ItemClass { get; private set; }
    public int Id { get; private set; }
    public int Count { get; private set; }
    public int Level { get; private set; }
    public bool IsEquipped { get; private set; }

    public ItemSaveData(
        ItemClass itemClass,
        int id,
        int count,
        int level,
        bool isEquipped)
    {
        ItemClass = itemClass;
        Id = id;
        Count = count;
        Level = level;
        IsEquipped = isEquipped;
    }
}