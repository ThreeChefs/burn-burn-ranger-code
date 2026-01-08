using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Monster", menuName = "SO/Monster/Monster")]
public class MonsterPoolObjectData : PoolObjectData
{
    [SerializeField] private MonsterTypeData _monsterTypeData;

    public MonsterTypeData MonsterData => _monsterTypeData;
}
