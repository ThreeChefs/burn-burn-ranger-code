public struct BaseEffectContext
{
    public TriggerReason Reason;
}

public struct KillEffectContext
{
    public BaseEffectContext Base;
    public KillStatus KillStatus;
}