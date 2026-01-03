
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New StatData", menuName = "Stats/Character Stats")]
public class StatData : ScriptableObject
{
   public List<StatEntry> stats;
}

[System.Serializable]
  public class StatEntry
  {
    public StatType statType;
    public float baseStat;
  }