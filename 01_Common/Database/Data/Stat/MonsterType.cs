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
    GreenGEM,
    BlueGEM,
    YellowGEM


}
[CreateAssetMenu(fileName = "MonsterTypeData", menuName = "Monster/Monster Type", order = 1)]
public class MonsterTypeData : StatData
{
    public MonsterType monsterType;




    [Header("Visual / Prefab")]
    public GameObject prefab;

    [Header("Drop")]
    public DropItemType dropItemType;
    public int dropCount = 1;
}

