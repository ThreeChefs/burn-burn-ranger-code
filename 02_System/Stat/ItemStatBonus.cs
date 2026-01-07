using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStatBonus
{
    public StatType statType;
    public StatModifierType statModifierType;
    public float value;
}

[CreateAssetMenu(fileName = "ItemStatBonusData", menuName = "SO/Stats/Character Bonus Stats", order = 1)]
public class StatItemData : ScriptableObject
{
    public List<ItemStatBonus> bonuses;
}