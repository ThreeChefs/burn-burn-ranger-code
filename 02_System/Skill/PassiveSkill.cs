public class PassiveSkill : BaseSkill
{
    private PassiveSkillData _passiveSkillData;

    public override void Init(SkillData data)
    {
        _passiveSkillData = data as PassiveSkillData;
        base.Init(data);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// [public] 레벨업
    /// 패시브 스킬이 적용되는 스텟에 해당 level value 데이터 저장
    /// </summary>
    public override void LevelUp()
    {
        base.LevelUp();

        PlayerStat stat = PlayerManager.Instance.Condition[_passiveSkillData.StatType];
        stat.UpdateBuffValue((stat.BaseValue == 0 ? 1 : stat.BaseValue) * skillValues[SkillValueType.StatBuff][0]);
    }
}
