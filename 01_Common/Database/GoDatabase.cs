using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject 데이터베이스
/// </summary>
[CreateAssetMenu(fileName = "GoDatabase", menuName = "SO/Database")]
public class GoDatabase : ScriptableObject
{
    [SerializeField] private List<GameObject> _list = new();

    public List<GameObject> GetDataTable()
    {
        List<GameObject> newList = new();

        foreach (GameObject go in _list)
        {
            if (go != null)
            {
                newList.Add(go);
            }
        }

        return newList;
    }
}
