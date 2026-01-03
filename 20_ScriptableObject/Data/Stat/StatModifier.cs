using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatModifierType
{
    Flat,       // 고정 수치 증가  
    PercentAdd, // 퍼센테이지 증가
}
public class StatModifier 
{
    public StatSortType statSortType;
    public StatModifierType statModifierType;
    public float value;
    public Object source; // 버프를 적용시킬 아이템 

    public StatModifier(StatModifierType statModifierType, float value, StatSortType statSortType, Object source)
    {
        this.statModifierType = statModifierType;
        this.value = value;
        this.statSortType = statSortType;
        this.source = source;
    }
}
