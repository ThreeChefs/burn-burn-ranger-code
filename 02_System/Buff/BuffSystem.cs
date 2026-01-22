using System.Collections.Generic;

/// <summary>
/// 버프 시스템
/// </summary>
public class BuffSystem
{
    private readonly List<BuffInstance> _active = new();
    private readonly PlayerCondition _condition;

    public BuffSystem(PlayerCondition condition)
    {
        _condition = condition;
    }

    public void Add(BuffInstanceKey key, BaseBuff buff)
    {
        BuffInstance existing = null;
        for (int i = 0; i < _active.Count; i++)
        {
            if (_active[i].Key.Equals(key))
            {
                existing = _active[i];
            }
        }

        if (existing != null)
        {
            ResolveStack(existing, buff);
            return;
        }

        BuffInstance instance = new(key, buff);
        _active.Add(instance);
        instance.Activate(_condition);
    }

    public void Update(float dt)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var instance = _active[i];
            instance.Tick(dt);

            if (instance.IsExpired)
            {
                Remove(instance);
            }
        }
    }

    public void OnHpChanged(float hpRatio)
    {
        foreach (BuffInstance instance in _active)
        {
            if (instance.Source is not IHpRatioReactiveBuff reactive) { continue; }

            if (reactive.ShouldBeActive(hpRatio))
            {
                instance.Activate(_condition);
            }
            else
            {
                instance.Deactive(_condition);
            }
        }
    }

    public void OnPlayerHit()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            if (_active[i].ShouldRemoveOnHit())
            {
                Remove(_active[i]);
            }
        }
    }

    private void Remove(BuffInstance instance)
    {
        instance.Deactive(_condition);
        _active.Remove(instance);
    }

    private void ResolveStack(BuffInstance existing, BaseBuff incoming)
    {
        switch (incoming.StackPolicy)
        {
            case BuffStackPolicy.Refresh:
                existing.Refresh();
                break;
            case BuffStackPolicy.Stack:
                existing.StackTime(incoming.BaseDuration);
                break;
            case BuffStackPolicy.Replace:
                Remove(existing);
                Add(existing.Key, incoming);
                break;
            case BuffStackPolicy.Ignore:
                break;
        }
    }
}
