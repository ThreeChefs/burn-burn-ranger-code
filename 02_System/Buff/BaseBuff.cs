public abstract class BaseBuff
{
    public float BaseDuration { get; protected set; }
    public BuffEndCondition EndCondition { get; protected set; }
    public BuffStackPolicy StackPolicy { get; protected set; }

    protected BaseBuff(float baseDuration, BuffEndCondition endCondition, BuffStackPolicy policy)
    {
        BaseDuration = baseDuration;
        EndCondition = endCondition;
        StackPolicy = policy;
    }

    public virtual void OnApply(PlayerCondition condition) { }
    public virtual void OnRemove(PlayerCondition condition) { }
    public virtual void OnUpdate(float dt) { }
}
