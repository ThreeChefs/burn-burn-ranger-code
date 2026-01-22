using System.Collections.Generic;
using System.Linq;

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
        var existing = _active.FirstOrDefault(b => b.Key.Equals(key));

        if (existing != null)
        {
            ResolveStack(existing, buff);
            return;
        }

        BuffInstance instance = new(key, buff);
        _active.Add(instance);
        buff.OnApply(_condition);
    }

    public void Update(float dt)
    {
        for (int i = 0; i < _active.Count; i++)
        {
            var instance = _active[i];
            instance.Source.OnUpdate(_condition, dt);
            instance.Tick(dt);

            if (instance.IsExpired)
            {
                Remove(instance);
            }
        }
    }

    private void Remove(BuffInstance instance)
    {
        instance.Source.OnRemove(_condition);
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
