using UnityEngine;

[System.Serializable]
public struct StatModifier
{
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public StatModifierType StatModifierType { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Value { get; private set; }

    public StatModifier(StatType type, StatModifierType statModifierType, float value)
    {
        StatType = type;
        StatModifierType = statModifierType;
        Value = value;
    }
}
