using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 인벤토리
/// </summary>
[Serializable]
public class Inventory
{
    [SerializeField] private List<ItemInstance> _items = new();
    public IReadOnlyList<ItemInstance> Items => _items;

    public event Action OnInventoryChanged;

    public const string DefaultWeapon = "쿠나이";

    // todo: 장비 아이템은 따로 관리
    // 현재는 테스트용으로 열기 -> private set으로 닫아두기
    public Dictionary<int, int> RequiredSkills = new();

    // 캐싱
    private Equipment _equipment;

    public void Init()
    {
        _equipment = PlayerManager.Instance.Equipment;

        if (_items.Count > 0) return;

        // 기본 무기 제공 및 장비
        ItemData data = GameManager.Instance.ItemDatabase.FindFirstByName(DefaultWeapon);
        ItemInstance instance = new(ItemClass.Normal, data);
        Add(instance);

        _equipment.Equip(instance);
    }

    /// <summary>
    /// [public] 플레이어가 파괴될 때 실행
    /// </summary>
    public void OnDestroy()
    {
        OnInventoryChanged = null;
    }

    /// <summary>
    /// [public] 아이템 추가
    /// </summary>
    /// <param name="item"></param>
    public void Add(ItemInstance item)
    {
        _items.Add(item);
        // todo: 나중에 정렬 로직 빼기
        _items.Sort((a, b) =>
        {
            // 클래스
            int result = b.ItemClass.CompareTo(a.ItemClass);
            if (result != 0)
            {
                return result;
            }

            // 장비 타입
            result = b.ItemData.EquipmentType.CompareTo(a.ItemData.EquipmentType);
            if (result != 0)
            {
                return result;
            }

            // 장비 아이디
            return b.ItemData.Id.CompareTo(a.ItemData.Id);
        });

        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// [public] 아이템 제거
    /// </summary>
    /// <param name="item"></param>
    public void Remove(ItemInstance item)
    {
        _items.Remove(item);

        OnInventoryChanged?.Invoke();
    }

    #region 합성
    public ItemInstance Compose(ItemInstance[] items)
    {
        if (items == null || items.Length < ItemUtils.ComposeRequiringCount) return null;

        for (int i = 0; i < ItemUtils.ComposeRequiringCount; i++)
        {
            if (PlayerManager.Instance.Equipment.IsEquip(items[i]))
            {
                PlayerManager.Instance.Equipment.Unequip(items[i]);
            }
            Remove(items[i]);
        }

        ItemInstance newItem = new(items[0].ItemClass + 1, items[0].ItemData);
        Add(newItem);

        return newItem;
    }

    public List<ItemInstance> ComposeAll()
    {
        List<ItemInstance> tempItems = new();
        List<ItemInstance> results = new();

        int id = -1;
        ItemClass itemClass = ItemClass.None;

        int count = 0;

        for (int i = _items.Count - 1; i >= 0;)
        {
            if (_items[i].ItemClass == ItemClass.Legendary)
            {
                i--;
                continue;
            }

            if (_items[i].ItemData.Id != id || _items[i].ItemClass != itemClass)
            {
                tempItems.Clear();
                count = 0;
            }

            tempItems.Add(_items[i]);
            count++;

            if (count == ItemUtils.ComposeRequiringCount &&
                tempItems.Count == ItemUtils.ComposeRequiringCount)
            {
                ItemInstance newItem = Compose(tempItems.ToArray());
                if (newItem != null)
                {
                    results.Add(newItem);
                    tempItems.Clear();
                    count = 0;
                    continue;
                }
            }

            id = _items[i].ItemData.Id;
            itemClass = _items[i].ItemClass;
            i--;
        }

        // results에서 재료 아이템으로 사용된 애 있으면 빼기
        for (int i = results.Count - 1; i >= 0; i--)
        {
            if (_items.Contains(results[i])) continue;
            results.Remove(results[i]);
        }

        return results;
    }
    #endregion

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
