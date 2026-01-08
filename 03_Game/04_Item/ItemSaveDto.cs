/// <summary>
/// 아이템 저장 시 사용할 dto
/// </summary>
public class ItemSaveDto
{
    public int Id { get; }
    public int Level { get; }
    public ItemClass ItemClass { get; }

    public ItemSaveDto(int id, int level, ItemClass itemClass)
    {
        Id = id;
        Level = level;
        ItemClass = itemClass;
    }
}
