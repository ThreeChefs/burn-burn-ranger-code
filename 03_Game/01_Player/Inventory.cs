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

    // todo: 장비 아이템은 따로 관리
    // 현재는 테스트용으로 열기 -> private set으로 닫아두기
    public Dictionary<int, int> RequiredSkills = new()
    {
        { 30, 1 }
    };

    public void Init()
    {
        if (_items.Count > 0) return;

        // todo: 나중에 인벤토리 초기화하기
        List<ItemData> defaultData = new()
        {
            GameManager.Instance.ItemDatabase.FindById(UnityEngine.Random.Range(0, 5)),
            GameManager.Instance.ItemDatabase.FindById(UnityEngine.Random.Range(0, 5)),
            GameManager.Instance.ItemDatabase.FindById(UnityEngine.Random.Range(0, 5)),
            GameManager.Instance.ItemDatabase.FindById(UnityEngine.Random.Range(0, 5)),
            GameManager.Instance.ItemDatabase.FindById(UnityEngine.Random.Range(0, 5)),
        };

        defaultData.ForEach(data => Add(
            new ItemInstance((ItemClass)UnityEngine.Random.Range(1, 5), data)));
    }

    /// <summary>
    /// [public] 플레이어가 파괴될 때 실행
    /// </summary>
    public void OnDestroy()
    {
        OnInventoryChanged = null;
        // todo: 아이템 데이터 저장
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
            int result = b.ItemClass.CompareTo(a.ItemClass);
            if (result != 0)
                return result;

            return b.ItemData.EquipmentType.CompareTo(a.ItemData.EquipmentType);
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
