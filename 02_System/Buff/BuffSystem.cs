using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 버프 시스템
/// </summary>
public class BuffSystem
{
    private readonly List<BuffInstance> _active = new();
    private readonly StagePlayer _player;

    public BuffSystem(StagePlayer player)
    {
        _player = player;
    }

    public void Add(BaseBuff buff)
    {
        var existing = _active.FirstOrDefault(b => b.Source.Id == buff.Id);

        if (existing != null)
        {
            ResolveStack(existing, buff);
            return;
        }

        BuffInstance instance = new(buff);
        _active.Add(instance);
        buff.OnApply(_player);
    }

    public void Update(float dt)
    {
        for (int i = 0; i < _active.Count; i++)
        {
            var instance = _active[i];
            instance.Source.OnUpdate(_player, dt);
            instance.Tick(dt);

            if (instance.IsExpired)
                Remove(instance);
        }
    }

    private void Remove(BuffInstance inst)
    {
        inst.Source.OnRemove(_player);
        _active.Remove(inst);
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
                Add(incoming);
                break;

            case BuffStackPolicy.Ignore:
                break;
        }
    }
}
