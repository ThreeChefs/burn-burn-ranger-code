using System.Collections.Generic;

public class InventorySaveData
{
    public List<ItemSaveData> List { get; set; } = new();
}

public struct ItemSaveData
{
    public ItemClass ItemClass { get; set; }
    public int Id { get; set; }
    public int Count { get; set; }
    public int Level { get; set; }
    public bool IsEquipped { get; set; }

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