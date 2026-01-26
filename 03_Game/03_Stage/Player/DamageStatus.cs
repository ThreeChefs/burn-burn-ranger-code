using System.Collections.Generic;

public class DamageStatus
{
    // int: skill id
    // float: damage
    public readonly Dictionary<int, float> _totalDamage;

    public DamageStatus()
    {
        _totalDamage = new();
    }

    public void Add(int id, float value)
    {
        if (!_totalDamage.ContainsKey(id))
        {
            _totalDamage.Add(id, 0);
        }

        _totalDamage[id] += value;
        //Logger.Log($"스킬 {id} 현재 total damage: {_totalDamage[id]}");
    }
}
