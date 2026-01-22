public class HpConditionStatBuff : BaseBuff, IHpRatioReactiveBuff
{
    private HpRatioCondition _condition;
    private StatModifier _modifier;
    private float _value;

    public HpConditionStatBuff(
        HpRatioCondition condition,
        StatModifier modifier,
        BuffEndCondition endCondition) : base(float.PositiveInfinity, endCondition, BuffStackPolicy.Ignore)
    {
        _condition = condition;
        _modifier = modifier;
        float statValue = PlayerManager.Instance.Condition[_modifier.StatType].BaseValue;
        if (statValue == 0)
        {
            statValue = 1;
        }
        _value = _modifier.StatModifierType == StatModifierType.Flat
            ? _modifier.Value
            : statValue * _modifier.Value * 0.01f;
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
        base.OnApply(condition);
        condition[_modifier.StatType].UpdateBuffValue(_value);
    }

    public override void OnRemove(PlayerCondition condition)
    {
        base.OnRemove(condition);
        condition[_modifier.StatType].UpdateBuffValue(-_value);
    }
}
