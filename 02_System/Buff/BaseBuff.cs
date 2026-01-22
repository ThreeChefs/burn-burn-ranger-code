public abstract class BaseBuff
{
    public float BaseDuration { get; protected set; }
    public BuffStackPolicy StackPolicy { get; protected set; }

    protected BaseBuff(float baseDuration, BuffStackPolicy policy)
    {
        BaseDuration = baseDuration;
        StackPolicy = policy;
    }

    public virtual void OnApply(StagePlayer player) { }
    public virtual void OnRemove(StagePlayer player) { }
    public virtual void OnUpdate(StagePlayer player, float dt) { }
}
