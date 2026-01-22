public sealed class BuffInstance
{
    public BaseBuff Source { get; }
    public float RemainingTime { get; private set; }

    public BuffInstance(BaseBuff source)
    {
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
