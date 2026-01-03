using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [SerializeField] private StatData baseStatData;

    private readonly Dictionary<StatSortType, float> _base = new();

    private readonly List<StatModifier> _mods = new();

    private void Awake()
    {
        BuildBase();
    }
    private void BuildBase()
    {
        _base.Clear();

        if (baseStatData == null|| baseStatData.stats == null) 
            return;
        foreach (var e in baseStatData.stats)
        {
            _base[e.statSortType] = e.baseStat;
        }

    }

    public float BaseStat(StatSortType statSortType)
        => _base.TryGetValue(statSortType, out var value) ? value : 0f;

    public float FinalStat(StatSortType statSortType)
    {
        float value = BaseStat(statSortType);
        float percentAdd = 0f;

        for(int i = 0; i< _mods.Count; i++)
        {
            var modifier = _mods[i];

            if(modifier.statSortType != statSortType)
                continue;
            if(modifier.statModifierType == StatModifierType.Flat)
            {
                value += modifier.value;
            }
            else if (modifier.statModifierType == StatModifierType.PercentAdd)
            {
                percentAdd += modifier.value;
            }
           
        }
        value *= (1f + percentAdd);

        return value;
    }
    public void AddModifier(StatModifier modifier)
    {
        _mods.Add(modifier);
    }

    public void ReMoveSourceModifiers(Object source)
    {
        _mods.RemoveAll(m => m.source == source);
    }

    public float GetPickItemStat(float baseRadius)  
    {
        float bonus = FinalStat(StatSortType.DropItemRange); // 자석 증가율
        return baseRadius * (1f + bonus);                   // 최종 반경
    }
}
