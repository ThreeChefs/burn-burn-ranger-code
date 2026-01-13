using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "SO/Database/Skill Database")]
public class SkillDatabase : SoDatabase
{
#if UNITY_EDITOR
    private void Reset()
    {
        var skills = AssetLoader.FindAndLoadAllByType<SkillData>();
        skills.Sort((a, b) => a.Id.CompareTo(b.Id));
        skills.ForEach(skill => List.Add(skill));
    }

    [Button("스킬 데이터 자동 추가")]
    private void AutoAddSkillData()
    {
        var skills = AssetLoader.FindAndLoadAllByType<SkillData>();
        foreach (var skill in skills)
        {
            if (!List.Contains(skill))
            {
                if (skill.Type != SkillType.Passive)
                {
                    AutoAddProjectileData(((ActiveSkillData)skill).ProjectileData);
                }
                List.Add(skill);
            }
        }
    }

    private void AutoAddProjectileData(ProjectileData data)
    {
        if (data == null) return;

        var db = AssetLoader.FindAndLoadByName<PoolObjectDatabase>("ProjectileDatabase");
        if (!db.List.Contains(data))
        {
            db.List.Add(data);
        }
    }

    [Button("아이디로 정렬")]
    private void SortById()
    {
        var skills = GetDatabase<SkillData>();
        List.Clear();
        skills.Sort((a, b) => a.Id.CompareTo(b.Id));
        skills.ForEach(skill => List.Add(skill));
    }
#endif
}
