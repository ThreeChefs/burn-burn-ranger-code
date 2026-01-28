using UnityEngine;

public enum MonsterType
{
    [Header("Monster Type")]
    Normal,
    Miniboss,
    Boss,
}

public enum DropItemType
{
    GreenGem,
    BlueGem,
    PurpleGem,
}

[CreateAssetMenu(fileName = "MonsterTypeData", menuName = "SO/Monster/Monster Type", order = 1)]
public class MonsterTypeData : StatData
{
    public MonsterType monsterType;



    [Header("Drop")]
    public GemPoolIndex gemPoolIndex;
    public int dropCount = 1;

    private void Reset()
    {
        Stats = new()
        {
            new StatEntry(StatType.Speed, 5f),
            new StatEntry(StatType.Attack, 5f),
            new StatEntry(StatType.Health, 5f)
        };
    }
}

