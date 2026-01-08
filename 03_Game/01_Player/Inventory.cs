using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 인벤토리
/// </summary>
[System.Serializable]
public class Inventory
{
    private List<ItemInstance> _items = new();
    public IReadOnlyList<ItemInstance> Items => _items;

    /// <summary>
    /// [public] 플레이어가 파괴될 때 실행
    /// </summary>
    public void OnDestroy()
    {
        // todo: 아이템 데이터 저장
    }

    /// <summary>
    /// [public] 아이템 추가
    /// </summary>
    /// <param name="item"></param>
    public void Add(ItemInstance item)
    {
        _items.Add(item);
    }

    /// <summary>
    /// [public] 아이템 제거
    /// </summary>
    /// <param name="item"></param>
    public void Remove(ItemInstance item)
    {
        _items.Remove(item);
    }

    // todo: Comparer 정렬으로 변경
    #region 정렬
    public void SortByClass()
    {
        _items = _items
            .OrderBy(item => item.ItemClass)
            .ThenBy(item => item.ItemData.Id)
            .ToList();
    }

    public void SortByLevel()
    {
        _items = _items
            .OrderBy(item => item.Level)
            .ThenBy(item => item.ItemClass)
            .ThenBy(item => item.ItemData.Id)
            .ToList();
    }

    public void SortByEquipmentType()
    {
        _items = _items
            .OrderBy(item => item.ItemData.EquipmentType)
            .ThenBy(item => item.ItemClass)
            .ThenBy(item => item.ItemData.Id)
            .ToList();
    }
    #endregion
}
