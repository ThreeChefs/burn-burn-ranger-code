public sealed class BuffInstance
{
    public BuffInstanceKey Key { get; }
    public BaseBuff Source { get; }
    public float RemainingTime { get; private set; }

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

    public void Tick(float dt)
    {
        RemainingTime -= dt;
    }

    public bool IsExpired => RemainingTime <= 0f;
}
