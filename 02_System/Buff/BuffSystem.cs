using System.Collections.Generic;
using System.Linq;

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

        var inst = new BuffInstance(buff);
        _active.Add(inst);
        buff.OnApply(_player);
    }

    public void Update(float dt)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var inst = _active[i];
            inst.Source.OnUpdate(_player, dt);
            inst.Tick(dt);

            if (inst.IsExpired)
                Remove(inst);
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
