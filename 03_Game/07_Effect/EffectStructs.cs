public struct BaseEffectContext
{
    public TriggerReason Reason;
    public BuffSystem BuffSystem;
}

public struct KillEffectContext
{
    public BaseEffectContext Base;
    public KillStatus KillStatus;
}

[System.Serializable]
public struct KillIntervalCondition
{
    public MonsterType Type;
    public int Interval;
}
