using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatData", menuName = "SO/Stats/Stat Data")]
public class StatData : ScriptableObject
{
    [field: SerializeField]
    public List<StatEntry> Stats { get; protected set; }

    private Dictionary<StatType, float> _statDict;

    private void Reset()
    {
        BuildCache();
    }

    private void BuildCache()
    {
        _statDict = new Dictionary<StatType, float>();

        if (Stats == null) return;

        foreach (var entry in Stats)
        {
            if (_statDict.ContainsKey(entry.StatType))
            {
                Debug.LogWarning($"[StatData] 중복 StatType 발견: {entry.StatType}", this);
                continue;
            }

            _statDict.Add(entry.StatType, entry.BaseValue);
        }
    }

    public float Get(StatType type, float defaultValue = 0f)
    {
        if (_statDict == null)
            BuildCache();

        return _statDict.TryGetValue(type, out var value)
            ? value
            : defaultValue;
    }
}

[System.Serializable]
public class StatEntry
{
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public float BaseValue { get; private set; }

    public StatEntry(StatType type, float value)
    {
        StatType = type;
        BaseValue = value;
    }
}