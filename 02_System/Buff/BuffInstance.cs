public sealed class BuffInstance
{
    public BuffInstanceKey Key { get; }
    public BaseBuff Source { get; }
    public float RemainingTime { get; private set; }
    public bool IsActive { get; private set; }
    private BuffEndCondition _endCondition;

    public BuffInstance(BuffInstanceKey key, BaseBuff source)
    {
        Key = key;
        Source = source;
        RemainingTime = source.BaseDuration;
        _endCondition = source.EndCondition;
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
        if ((_endCondition & BuffEndCondition.Time) == 0) return;
        if (!IsActive) return;

        RemainingTime -= dt;
    }

    /// <summary>
    /// 버프 활성화
    /// </summary>
    /// <param name="condition"></param>
    public void Activate(PlayerCondition condition)
    {
        if (IsActive) return;
        IsActive = true;
        Source.OnApply(condition);
    }

    /// <summary>
    /// 버프 비활성화
    /// </summary>
    /// <param name="condition"></param>
    public void Deactive(PlayerCondition condition)
    {
        if (!IsActive) return;
        IsActive = false;
        Source.OnRemove(condition);
    }

    /// <summary>
    /// 충돌 시 버프가 해제되는지 확인
    /// </summary>
    /// <returns></returns>
    public bool ShouldRemoveOnHit()
    {
        return (_endCondition & BuffEndCondition.BreakOnHit) != 0;
    }

    public bool IsExpired => RemainingTime <= 0f;
}
