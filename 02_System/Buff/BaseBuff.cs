public abstract class BaseBuff
{
    public float BaseDuration { get; protected set; }
    public BuffStackPolicy StackPolicy { get; protected set; }

    protected BaseBuff(float baseDuration, BuffStackPolicy policy)
    {
        BaseDuration = baseDuration;
        StackPolicy = policy;
    }

    public virtual void OnApply(PlayerCondition condition) { }
    public virtual void OnRemove(PlayerCondition condition) { }
    public virtual void OnUpdate(PlayerCondition condition, float dt) { }
}
