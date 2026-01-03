using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatData", menuName = "SO/Stats/Character Stats")]
public class StatData : ScriptableObject
{
    [field: SerializeField] public List<StatEntry> Stats { get; private set; }
}

[System.Serializable]
public class StatEntry
{
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public float BaseValue { get; private set; }
}