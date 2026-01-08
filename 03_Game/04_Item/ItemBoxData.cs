using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemBoxData", menuName = "SO/Item/Item Box")]
public class ItemBoxData : ScriptableObject
{
    [field: SerializeField] public List<ItemBoxEntry> Items { get; private set; }
}

[System.Serializable]
public class ItemBoxEntry
{
    [field: SerializeField] public ItemClass ItemClass { get; private set; }
    [field: SerializeField] public int Weight { get; private set; }
    [field: SerializeField] public List<ItemData> Items { get; private set; }

#if UNITY_EDITOR
    [Button("정렬")]
    private void SortItems()
    {
        Items = Items.OrderBy(item => item.EquipmentType)
            .ThenBy(item => item.Id)
            .ToList();
    }
#endif
}