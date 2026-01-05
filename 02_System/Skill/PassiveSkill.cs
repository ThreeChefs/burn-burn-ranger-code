public class PassiveSkill : BaseSkill
{
    private PassiveSkillData _passiveSkillData;

    public override void Init(SkillData data)
    {
        base.Init(data);

        _passiveSkillData = data as PassiveSkillData;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerManager.Instance.Condition[_passiveSkillData.StatType]
            .UpdateBuffValue(0);
    }

    /// <summary>
    /// [public] 레벨업
    /// 패시브 스킬이 적용되는 스텟에 해당 level value 데이터 저장
    /// </summary>
    public override void LevelUp()
    {
        base.LevelUp();

        PlayerManager.Instance.Condition[_passiveSkillData.StatType]
            .UpdateBuffValue(_passiveSkillData.LevelValue[CurLevel - 1]);
    }
}
