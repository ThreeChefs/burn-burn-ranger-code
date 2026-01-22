public abstract class BaseBuff
{
    public BuffId Id { get; }
    public float BaseDuration { get; protected set; }
    public BuffStackPolicy StackPolicy { get; protected set; }

    protected BaseBuff(BuffId id, float baseDuration, BuffStackPolicy policy)
    {
        Id = id;
        BaseDuration = baseDuration;
        StackPolicy = policy;
    }

    public virtual void OnApply(StagePlayer player) { }
    public virtual void OnRemove(StagePlayer player) { }
    public virtual void OnUpdate(StagePlayer player, float dt) { }
}
