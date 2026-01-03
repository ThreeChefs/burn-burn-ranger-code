using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 선택 및 부여를 관리하는 시스템
/// </summary>
public class SkillSystem : MonoBehaviour
{
    [SerializeField] private SoDatabase _skillDatabase;
    private List<SkillData> _cache;

    // 스킬 상태 관리
    protected readonly Dictionary<int, BaseSkill> havingSkills = new();
    protected int activeSkillCount;
    protected int passiveSkillCount;

    private void Awake()
    {
        _cache = _skillDatabase.GetDatabase<SkillData>();
    }

    // todo list
    // 1. 스킬 선택하기
    // - 등급과 설명 보여주기
    // -- 보유 스킬일 경우 / 보유하지 않을 경우 
    // 2. 스킬 보관하기
    // 

    /// <summary>
    /// [public] 스킬 선택하기
    /// </summary>
    /// <param name="id"></param>
    public bool TrySelectSkill<T>(int id) where T : BaseSkill
    {
        if (_cache.Count < id)
        {
            Logger.LogWarning($"얻을 수 없는 스킬 데이터: {id}");
            return false;
        }

        if (havingSkills.TryGetValue(id, out var skill))
        {
            skill.LevelUp();
            return true;
        }

        T baseSkill = gameObject.AddComponent<T>();
        havingSkills.Add(id, baseSkill);
        SkillData data = _cache[id];
        baseSkill.Init(data);

        switch (data.Type)
        {
            case SkillType.Active:
            case SkillType.Combination:
                activeSkillCount++;
                break;
            case SkillType.Passive:
                passiveSkillCount++;
                break;
        }

        return true;
    }

    #region 에디터 전용
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        if (_skillDatabase == null)
        {
            _skillDatabase = AssetLoader.FindAndLoadByName<SoDatabase>("PlayerSkillDatabase");
        }
    }
#endif
    #endregion
}
