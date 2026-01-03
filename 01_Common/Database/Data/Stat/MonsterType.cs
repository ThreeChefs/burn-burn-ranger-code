using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Normal,
    
    Miniboss,
    Boss
}
[CreateAssetMenu(fileName = "MonsterTypeData", menuName = "Monster/Monster Type", order = 1)]
public class MonsterTypeData : ScriptableObject
{
    public MonsterType monsterType;

    [Header("Base Stat")]
    public float baseHealth;
    public float baseAttack;
    public float baseSpeed;



    [Header("Visual / Prefab")]
    public GameObject prefab;
}

