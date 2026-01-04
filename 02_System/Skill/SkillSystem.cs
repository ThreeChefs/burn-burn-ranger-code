using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 선택 및 부여를 관리하는 시스템
/// </summary>
public class SkillSystem
{
    private SoDatabase _skillDatabase;
    private readonly Dictionary<int, SkillData> _skillDataCache = new();

    // 스킬 상태 관리
    private readonly Dictionary<int, BaseSkill> _ownedSkills = new();
    private readonly Dictionary<int, int> _combinationRequirementMap = new();
    private readonly List<int> _selectableOwnedSkillIds = new();
    private int _activeSkillCount;
    private int _passiveSkillCount;

    private readonly StagePlayer _player;

    #region 초기화
    public SkillSystem(SoDatabase skillDatabase, StagePlayer player)
    {
        _skillDatabase = skillDatabase;
        _player = player;

        Init();
    }

    private void Init()
    {
        // 딕셔너리 초기화
        _skillDataCache.Clear();
        _selectableOwnedSkillIds.Clear();
        _skillDatabase.GetDatabase<SkillData>()
            .ForEach(skillData =>
            {
                _skillDataCache[skillData.Id] = skillData;
            });
        _ownedSkills.Clear();
        _combinationRequirementMap.Clear();

        // todo: 기본 스킬 주기
        // ex. 쿠나이
    }
    #endregion

    /// <summary>
    /// [public] 스킬 선택하기
    /// </summary>
    /// <param name="id"></param>
    public bool TrySelectSkill(int id)
    {
        if (_skillDataCache.TryGetValue(id, out SkillData data))
        {
            Logger.LogWarning($"얻을 수 없는 스킬 데이터: {id}");
            return false;
        }

        // 이미 스킬이 있는 경우 레벨업
        if (_ownedSkills.TryGetValue(id, out var skill))
        {
            skill.LevelUp();
            UpdateSkillSelectCondition(id);
            return true;
        }

        // 없는 경우 스킬 획득
        BaseSkill baseSkill = (data.Type) switch
        {
            SkillType.Active => GetActiveSkill(),
            SkillType.Combination => GetCombinationSkill(id),
            SkillType.Passive => GetPassiveSkill(),
            _ => null
        };

        if (baseSkill == null)
        {
            Logger.Log("base skill 생성 실패");
            return false;
        }

        // 스킬 획득 후 초기화
        _ownedSkills.Add(id, baseSkill);
        _selectableOwnedSkillIds.Add(id);
        baseSkill.Init(data);
        UpdateSkillSelectCondition(id);

        return true;
    }

    private ActiveSkill GetActiveSkill()
    {
        _activeSkillCount++;
        return _player.gameObject.AddComponent<ActiveSkill>();
    }

    private PassiveSkill GetPassiveSkill()
    {
        _passiveSkillCount++;
        return _player.gameObject.AddComponent<PassiveSkill>();
    }

    private ActiveSkill GetCombinationSkill(int id)
    {
        foreach (int combinationId in _skillDataCache[id].CombinationIds)
        {
            if (_skillDataCache[combinationId].Type == SkillType.Active)
            {
                _activeSkillCount--;
            }
            else if (_skillDataCache[combinationId].Type == SkillType.Passive)
            {
                _passiveSkillCount--;
            }
            GameObject.Destroy(_ownedSkills[combinationId].gameObject);
        }

        _activeSkillCount++;
        return _player.gameObject.AddComponent<ActiveSkill>();
    }

    public List<SkillSelectDto> ShowSelectableSkills(int count)
    {
        List<SkillSelectDto> skillSelectDtos = new();

        foreach (KeyValuePair<int, int> combinationSkillTerm in _combinationRequirementMap)
        {
            if (combinationSkillTerm.Value == 2)
            {
                SkillData skillData = _skillDataCache[combinationSkillTerm.Key];
                skillSelectDtos.Add(new SkillSelectDto(
                    skillData.Id,
                    1,
                    skillData.name,
                    skillData.Description,
                    skillData.Sprite,
                    null));
            }
        }

        if (skillSelectDtos.Count == count)
        {
            return skillSelectDtos;
        }
        else if (skillSelectDtos.Count > count)
        {
            return skillSelectDtos.Random(count);
        }

        // todo: 스킬 전부 획득
        if (_activeSkillCount + _passiveSkillCount >= Define.ActiveSkillMaxCount + Define.PassiveSkillMaxCount)
        {

        }

        // todo: 스킬 전부 획득하지 않았을 경우

        return null;
    }

    /// <summary>
    /// id번 스킬 획득 시 스킬 획득 조건을 갱신합니다.
    /// </summary>
    /// <param name="id"></param>
    private void UpdateSkillSelectCondition(int id)
    {
        BaseSkill skill = _ownedSkills[id];
        SkillData data = _skillDataCache[id];

        switch (data.Type)
        {
            case SkillType.Active:
                // 액티브 스킬일 경우 최대 레벨일 때 잠금 해제
                if (skill.CurLevel == Define.SkillMaxLevel)
                {
                    _selectableOwnedSkillIds.Remove(id);                // 획득 불가능
                    ApplyCombinationSkillDict(data.CombinationIds); // 조합 스킬 조건 확인
                }
                break;
            case SkillType.Passive:
                // 패시브 스킬일 경우 1레벨일 때 잠금 해제
                if (skill.CurLevel == 1)
                {
                    ApplyCombinationSkillDict(data.CombinationIds); // 조합 스킬 조건 확인
                }
                else if (skill.CurLevel == Define.SkillMaxLevel)
                {
                    _selectableOwnedSkillIds.Remove(id);                // 획득 불가능
                }
                break;
            case SkillType.Combination:
                if (_combinationRequirementMap.ContainsKey(id))
                {
                    _combinationRequirementMap.Remove(id);
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
            if (_combinationRequirementMap.ContainsKey(combinationId))
            {
                _combinationRequirementMap[combinationId]++;
            }
            else
            {
                _combinationRequirementMap.Add(combinationId, 1);
            }
        }
    }
}
