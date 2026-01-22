public class StatBuff : BaseBuff
{
    private StatModifier _modifier;
    private float _value;

    public StatBuff(
        float baseDuration,
        StatModifier statModifier,
        BuffStackPolicy policy = BuffStackPolicy.Refresh) : base(baseDuration, policy)
    {
        _modifier = statModifier;
        _value = _modifier.StatModifierType == StatModifierType.Flat
            ? _modifier.Value
            : PlayerManager.Instance.Condition[_modifier.StatType].BaseValue * _modifier.Value * 0.01f;
    }

    public override void OnApply(PlayerCondition condition)
    {
        base.OnApply(condition);
        condition[_modifier.StatType].UpdateBuffValue(_value);
    }

    public override void OnRemove(PlayerCondition condition)
    {
        base.OnRemove(condition);
        condition[_modifier.StatType].UpdateBuffValue(-_value);
    }
}
