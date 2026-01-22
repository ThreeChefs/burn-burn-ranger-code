public sealed class BuffInstance
{
    public BuffInstanceKey Key { get; }
    public BaseBuff Source { get; }
    public float RemainingTime { get; private set; }
    public bool IsActive { get; private set; }

    public BuffInstance(BuffInstanceKey key, BaseBuff source)
    {
        Key = key;
        Source = source;
        RemainingTime = source.BaseDuration;
    }

    public void Refresh()
    {
        RemainingTime = Source.BaseDuration;
    }

    public void StackTime(float time)
    {
        RemainingTime += time;
    }

    public void Tick(PlayerCondition condition, float dt)
    {
        if (!IsActive) return;

        RemainingTime -= dt;
    }

    public void Activate(PlayerCondition condition)
    {
        if (IsActive) return;
        IsActive = true;
        Source.OnApply(condition);
    }

    public void Deactive(PlayerCondition condition)
    {
        if (!IsActive) return;
        IsActive = false;
        Source.OnRemove(condition);
    }

    public bool IsExpired => RemainingTime <= 0f;
}
