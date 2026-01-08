using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoolObjectData", menuName = "SO/Pool Object")]
public class PoolObjectData : ScriptableObject 
{
    [Title("Origin Prefab")]
    [SerializeField] private PoolObject _originPrefab;
    [SerializeField] private int _defaultPoolSize = 50;
    public int DefaultPoolSize => _defaultPoolSize;
    public PoolObject OriginPrefab => _originPrefab;

}
