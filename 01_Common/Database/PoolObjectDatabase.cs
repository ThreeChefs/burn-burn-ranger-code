using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolObject Database", menuName = "SO/Database/PoolObjectDatabase")]
public class PoolObjectDatabase : ScriptableObject
{
    [SerializeField] private List<PoolObjectData> _list = new();
    public List<PoolObjectData> List => _list;

    public List<T> GetDatabase<T>() where T : PoolObjectData
    {
        List<T> newList = new();

        foreach (ScriptableObject so in _list)
        {
            T t = so as T;
            if (t != null)
            {
                newList.Add(t);
            }
        }

        return newList;
    }
}
