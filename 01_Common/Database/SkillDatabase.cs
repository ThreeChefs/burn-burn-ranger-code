using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "SO/Database/Skill Database")]
public class SkillDatabase : SoDatabase
{
#if UNITY_EDITOR
    private void Reset()
    {
        var skills = AssetLoader.FindAndLoadAllByType<SkillData>();
        skills.Sort((a, b) =>
        {
            int compare = a.Type.CompareTo(b.Type);
            if (compare != 0)
            {
                return compare;
            }
            return a.name.CompareTo(b.name);
        });
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
                List.Add(skill);
            }
        }
    }

    [Button("아이디로 정렬")]
    private void SortById()
    {
        var skills = GetDatabase<SkillData>();
        List.Clear();
        skills.Sort((a, b) =>
        {
            int compare = a.Type.CompareTo(b.Type);
            if (compare != 0)
            {
                return compare;
            }
            return a.name.CompareTo(b.name);
        });
        skills.ForEach(skill => List.Add(skill));
    }
#endif
}
