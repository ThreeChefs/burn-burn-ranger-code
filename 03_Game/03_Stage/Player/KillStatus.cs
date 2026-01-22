using System.Collections.Generic;
using System.Linq;

public class KillStatus
{
    private readonly Dictionary<MonsterType, int> _kills = new();
    private readonly Dictionary<(MonsterType, int), int> _intervalCount = new();

    private IReadOnlyList<EquipmentEffectInstance> _effects;

    public void Init()
    {
        _kills.Clear();
        _intervalCount.Clear();

        _effects = PlayerManager.Instance.StagePlayer.Effects;
    }

    public void OnMonsterKilled(MonsterType type)
    {
        AddKill(type);

        KillEffectContext context = new()
        {
            Base = new BaseEffectContext() { Reason = TriggerReason.MonsterKilled },
            KillStatus = this
        };

        foreach (IThresholdEffect effect in _effects.OfType<IThresholdEffect>())
        {
            if (!effect.TryConsume(context))
            {
                continue;
            }


        }
    }

    public void AddKill(MonsterType type)
    {
        _kills.TryGetValue(type, out int cur);
        _kills[type] = cur + 1;
    }

    public int GetKillCount(MonsterType type)
    {
        return _kills.TryGetValue(type, out int count) ? count : 0;
    }

    public int ConsumeInterval(MonsterType type, int interval)
    {
        int count = GetKillCount(type);
        int shouldTrigger = count / interval;

        (MonsterType, int) key = (type, interval);
        _intervalCount.TryGetValue(key, out int already);

        int canTrigger = shouldTrigger - already;
        if (canTrigger <= 0)
        {
            return 0;
        }

        _intervalCount[key] = already + canTrigger;
        return canTrigger;
    }
}
