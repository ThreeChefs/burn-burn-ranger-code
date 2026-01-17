using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowthInfo
{
    [GUIColor(1f, 1f, 0f)]
    [SerializeField] StatType _statType;     /// 레퍼런스 기준 Health, Heal, Attack, Defense / 우리는 더 만들 수도 있지
    
    [SerializeField] float _value;


    public StatType StatType => _statType;
    public float Value => _value;


    public GrowthInfo(StatType statType, float value)
    {
        _statType = statType;
        _value = value;
    }
}

[Serializable]
public class GrowthInfoEntry
{
    [GUIColor(1, 0.6f, 0.4f)]
    [SerializeField, Delayed] int _unlockLevel;
    
    [SerializeField] List<GrowthInfo> _growthInfos;

    public int UnlockLevel => _unlockLevel;
    public List<GrowthInfo> GrowthInfos => _growthInfos;

    public GrowthInfoEntry(int unlockLevel)
    {
        _unlockLevel = unlockLevel;
        
        _growthInfos = new List<GrowthInfo>();
        _growthInfos.Add(new GrowthInfo(StatType.Attack, 2));
        _growthInfos.Add(new GrowthInfo(StatType.Health, 2));
        _growthInfos.Add(new GrowthInfo(StatType.Defense, 2));
        _growthInfos.Add(new GrowthInfo(StatType.Heal, 2));

    }
}