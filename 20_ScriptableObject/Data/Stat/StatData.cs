
using System.Collections.Generic;
using UnityEngine;

public enum StatSortType
{
    Health,  // 기본 체력
    Speed,   // 이동 속도
    Attack,  // 공격력
    Defense, // 방어력
    AttackRange,    //공격 범위
    AddEXP,        //획득 경험치 추가 8%
    DropItemRange, //  기본범위는 없음. 닿아야 습득. 아이템 습득 범위 100%
    AddGold,       //획득 골드 추가 6%
    MaxHealth,     //최대 체력 20%증가
    Heal,   //5초마다 체력 1%회복
    AttackCooltime, // 공격 쿨타임


}
[CreateAssetMenu(fileName = "New StatData", menuName = "Stats/Character Stats")]
public class StatData : ScriptableObject
{
   public List<StatEntry> stats;
}

[System.Serializable]
public class StatEntry
{
    public StatSortType statSortType;
    public float baseStat;
}