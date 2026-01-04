using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 선택 및 부여를 관리하는 시스템
/// </summary>
public class SkillSystem : MonoBehaviour
{
    [SerializeField] private SoDatabase _skillDatabase;
    private Dictionary<int, SkillData> _cache;

    // 스킬 상태 관리
    private readonly Dictionary<int, BaseSkill> _havingSkills = new();
    private readonly Dictionary<int, int> _combinationSkillTerms = new();
    private int _activeSkillCount;
    private int _passiveSkillCount;

    private void Awake()
    {
        _skillDatabase.GetDatabase<SkillData>()
            .ForEach(skillData => _cache[skillData.Id] = skillData);
        _havingSkills.Clear();
        _combinationSkillTerms.Clear();
    }

    /// <summary>
    /// [public] 스킬 선택하기
    /// </summary>
    /// <param name="id"></param>
    public bool TrySelectSkill(int id)
    {
        if (_cache.TryGetValue(id, out SkillData data))
        {
            Logger.LogWarning($"얻을 수 없는 스킬 데이터: {id}");
            return false;
        }

        if (_havingSkills.TryGetValue(id, out var skill))
        {
            skill.LevelUp();
            UpdateCombinationSkillCondition(id);
            return true;
        }

        BaseSkill baseSkill = (data.Type) switch
        {
            SkillType.Active => GetActiveSkill(),
            SkillType.Combination => GetCombinationSkill(),
            SkillType.Passive => GetPassiveSkill(),
            _ => null
        };

        if (baseSkill == null)
        {
            Logger.Log("base skill 생성 실패");
            return false;
        }

        _havingSkills.Add(id, baseSkill);
        baseSkill.Init(data);
        UpdateCombinationSkillCondition(id);

        return true;
    }

    private ActiveSkill GetActiveSkill()
    {
        _activeSkillCount++;
        return gameObject.AddComponent<ActiveSkill>();
    }

    private PassiveSkill GetPassiveSkill()
    {
        _passiveSkillCount++;
        return gameObject.AddComponent<PassiveSkill>();
    }

    private ActiveSkill GetCombinationSkill()
    {
        // todo: 획득한 id 검사해서 active 스킬 중에 제거해야할 것은 없애야 함
        _activeSkillCount++;
        return gameObject.AddComponent<ActiveSkill>();
    }

    public List<SkillSelectDto> ShowSelectableSkills(int count)
    {
        List<SkillSelectDto> skillUIDtos = new();

        // todo: 스킬 조합 가능한지 확인하기

        // 스킬 전부 획득
        if (_activeSkillCount + _passiveSkillCount >= Define.ActiveSkillMaxCount + Define.PassiveSkillMaxCount)
        {

        }

        return null;
    }

    /// <summary>
    /// id번 스킬 획득 시 조합 스킬 조건을 갱신합니다.
    /// </summary>
    /// <param name="id"></param>
    private void UpdateCombinationSkillCondition(int id)
    {
        BaseSkill skill = _havingSkills[id];
        SkillData data = _cache[id];

        switch (data.Type)
        {
            case SkillType.Active:
                // 액티브 스킬일 경우 최대 레벨일 때 잠금 해제
                if (skill.CurLevel == Define.SkillMaxLevel)
                {
                    ApplyCombinationSkillDict(data.CombinationIds);
                }
                break;
            case SkillType.Passive:
                // 패시브 스킬일 경우 1레벨일 때 잠금 해제
                if (skill.CurLevel == 1)
                {
                    ApplyCombinationSkillDict(data.CombinationIds);
                }
                break;
            case SkillType.Combination:
                if (_combinationSkillTerms.ContainsKey(id))
                {
                    _combinationSkillTerms.Remove(id);
                }
                break;
        }
    }

    /// <summary>
    /// 스킬 조합 조건 딕셔너리에 정보 저장
    /// </summary>
    /// <param name="combinationIds"></param>
    private void ApplyCombinationSkillDict(int[] combinationIds)
    {
        foreach (int combinationId in combinationIds)
        {
            if (_combinationSkillTerms.ContainsKey(combinationId))
            {
                _combinationSkillTerms[combinationId]++;
            }
            else
            {
                _combinationSkillTerms.Add(combinationId, 1);
            }
        }
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
