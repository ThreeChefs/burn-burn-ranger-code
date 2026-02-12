using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoolObjectData", menuName = "SO/Pool Object")]
public class PoolObjectData : ScriptableObject 
{
    [Title("PoolObject Data")]
    [SerializeField] private PoolObject _originPrefab;
    [SerializeField] private int _defaultPoolSize = 50;
    public int DefaultPoolSize => _defaultPoolSize;
    public PoolObject OriginPrefab => _originPrefab;

}
