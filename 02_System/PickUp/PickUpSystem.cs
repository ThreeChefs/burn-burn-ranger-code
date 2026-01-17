using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 픽업 시스템
/// </summary>
public class PickUpSystem
{
    // 캐싱
    private readonly Dictionary<int, ItemBoxData> _itemBoxCache;

    public PickUpSystem(SoDatabase itemBoxDatabase)
    {
        _itemBoxCache = new();
        itemBoxDatabase.GetDatabase<ItemBoxData>().ForEach(itemBoxData =>
        {
            if (_itemBoxCache.ContainsKey(itemBoxData.Id))
            {
                Logger.LogWarning($"중복 아이디: {itemBoxData.Id}");
            }
            _itemBoxCache.Add(itemBoxData.Id, itemBoxData);
        });
    }

    /// <summary>
    /// id번째 box에서 랜덤으로 아이템 1개 뽑기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemInstance PickUp(int id)
    {
        ItemBoxData itemBoxData = _itemBoxCache[id];

        float rand = Random.value * 100f;
        float cumulative = 0f;

        foreach (ItemBoxEntry entry in itemBoxData.ItemBoxEntries)
        {
            cumulative += entry.Weight;
            if (rand <= cumulative)
            {
                int index = Random.Range(0, entry.Items.Count);
                return new ItemInstance(entry.ItemClass, entry.Items[index]);
            }
        }

        return null;
    }
}
