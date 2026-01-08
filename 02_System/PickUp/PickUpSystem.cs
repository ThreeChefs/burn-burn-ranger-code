using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [field: Header("데이터베이스")]
    [field: SerializeField] private SoDatabase _itemBoxDatabase;

    // 캐싱
    private Dictionary<int, ItemBoxData> _itemBoxCache;

    private void Awake()
    {
        _itemBoxCache = new();
        ConvertToDict();
    }

    private void ConvertToDict()
    {
        _itemBoxDatabase.GetDatabase<ItemBoxData>().ForEach(itemBoxData =>
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
    public ItemData PickUp(int id)
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
                return entry.Items[index];
            }
        }

        return null;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _itemBoxDatabase = AssetLoader.FindAndLoadByName<SoDatabase>("ItemBoxDatabase");
    }
#endif
}
