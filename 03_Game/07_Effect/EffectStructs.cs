public struct BaseEffectContext
{
    public TriggerReason Reason;
}


public struct KillEffectContext
{
    public BaseEffectContext Base;
    public KillStats KillStats;
}

public struct KillStats
{

}