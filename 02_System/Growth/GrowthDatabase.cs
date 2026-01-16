using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GrowthDatabase", menuName = "SO/Database/GrowthDatabase")]
public class GrowthDatabase : ScriptableObject
{
    
    [SerializeField] List<GrowthInfoEntry> _growthInfoStep = new List<GrowthInfoEntry>();
    
    public List<GrowthInfoEntry> GrowthInfoStep => _growthInfoStep;


    private void OnValidate()
    {
        _growthInfoStep.Sort((a, b) =>
            a.UnlockLevel.CompareTo(b.UnlockLevel)
        );
    }


    [Button("초기화")]
    void AutoAdd()
    {
        _growthInfoStep= new List<GrowthInfoEntry>();   
        for (int i = 0; i < Define.PlayerMaxLevel; ++i)
        {
            GrowthInfoEntry entry = new GrowthInfoEntry(i);
            _growthInfoStep.Add(entry);
        }
    }
}

