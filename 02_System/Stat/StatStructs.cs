public struct StatModifier
{
    public StatType StatType { get; private set; }
    public StatModifierType StatModifierType { get; private set; }
    public float Value { get; private set; }

    public StatModifier(StatType type, StatModifierType statModifierType, float value)
    {
        StatType = type;
        StatModifierType = statModifierType;
        Value = value;
    }
}
