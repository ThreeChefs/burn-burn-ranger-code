using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GrowthDatabase", menuName = "SO/Database/GrowthDatabase")]
public class GrowthDatabase : ScriptableObject
{
    
    [SerializeField] List<GrowthInfoEntry> _growthInfoEntries = new List<GrowthInfoEntry>();
    
    public List<GrowthInfoEntry> GrowInfoEntries => _growthInfoEntries;


    private void OnValidate()
    {
        _growthInfoEntries.Sort((a, b) =>
            a.UnlockLevel.CompareTo(b.UnlockLevel)
        );
    }


    [Button("초기화")]
    void AutoAdd()
    {
        _growthInfoEntries= new List<GrowthInfoEntry>();   
        for (int i = 1; i < Define.PlayerMaxLevel; ++i)
        {
            GrowthInfoEntry entry = new GrowthInfoEntry(i);
            _growthInfoEntries.Add(entry);
        }
    }

    
}


