public class HpConditionStatBuff : BaseBuff, IHpRatioReactiveBuff
{
    private HpRatioCondition _condition;
    private StatModifier _modifier;
    private float _value;

    public HpConditionStatBuff(
        HpRatioCondition condition,
        StatModifier modifier) : base(float.PositiveInfinity, BuffStackPolicy.Ignore)
    {
        _condition = condition;
        _modifier = modifier;
        _value = _modifier.StatModifierType == StatModifierType.Flat
            ? _modifier.Value
            : PlayerManager.Instance.Condition[_modifier.StatType].BaseValue * _modifier.Value;
    }

    /// <summary>
    /// 활성화 가능한지 확인하기
    /// </summary>
    /// <param name="hpRatio"></param>
    /// <returns></returns>
    public bool ShouldBeActive(float hpRatio)
    {
        return _condition.Evaluate(hpRatio);
    }

    public override void OnApply(PlayerCondition condition)
    {
        condition[_modifier.StatType].UpdateBuffValue(_value);
    }

    public override void OnRemove(PlayerCondition condition)
    {
        condition[_modifier.StatType].UpdateBuffValue(-_value);
    }
}
