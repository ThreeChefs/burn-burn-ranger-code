public class HealBuff : BaseBuff
{
    public float HealPerSecond { get; }

    public HealBuff(
        float baseDuration,
        float healPerSecond,
        BuffStackPolicy policy = BuffStackPolicy.Refresh) : base(baseDuration, policy)
    {
        HealPerSecond = healPerSecond;
    }

    public override void OnUpdate(StagePlayer player, float dt)
    {
        float healAmount = HealPerSecond * dt;
        PlayerManager.Instance.Condition[StatType.Health].Add(healAmount);
    }
}
