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
        Logger.Log($"버프 생성: {instance.Source}");
        _active.Add(instance);
        instance.Activate(_condition);
    }

    public void Update(float dt)
    {
        for (int i = 0; i < _active.Count; i++)
        {
            var instance = _active[i];
            instance.Tick(_condition, dt);

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
            bool active = ((IHpRatioReactiveBuff)instance.Source).ShouldBeActive(hpRatio);

            if (active)
            {
                instance.Activate(_condition);
            }
            else
            {
                instance.Deactive(_condition);
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
