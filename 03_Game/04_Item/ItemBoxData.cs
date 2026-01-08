using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemBoxData", menuName = "SO/Item/Item Box")]
public class ItemBoxData : ScriptableObject
{
    [field: SerializeField] public List<ItemData> Items { get; private set; }
}
