using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDatabase", menuName = "SO/Database/Projectile Database")]
public class ProjectileDatabase : PoolObjectDatabase
{
#if UNITY_EDITOR
    private void Reset()
    {
        FindAndSortData(true);
    }

    private void FindAndSortData(bool sort)
    {
        var projectiles = AssetLoader.FindAndLoadAllByType<ProjectileData>();

        if (sort)
        {
            SortByName();
        }

        foreach (var data in projectiles)
        {
            if (!List.Contains(data))
            {
                List.Add(data);
            }
        }
    }

    [Button("투사체 데이터 자동 추가")]
    private void AutoAddSkillData()
    {
        FindAndSortData(false);
    }

    [Button("알파벳 순으로 정렬")]
    private void SortByName()
    {
        List.Sort((a, b) => a.name.CompareTo(b.name));
    }
#endif
}
