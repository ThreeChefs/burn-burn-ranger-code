using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 스킬 선택 및 부여를 관리하는 시스템
/// </summary>
public class SkillSystem
{
    private readonly SoDatabase _skillDatabase;
    private readonly Dictionary<int, SkillData> _skillDataCache = new();
    private readonly StagePlayer _player;

    // 스킬 상태 관리
    private readonly Dictionary<int, BaseSkill> _ownedSkills = new();
    private readonly Dictionary<int, int> _combinationRequirementMap = new();
    private readonly List<int> _maxedSkillIds = new();
    private int _activeSkillCount;
    private int _passiveSkillCount;

    private bool _canSelectSkill;
    private int TotalMaxSkillCount => Define.ActiveSkillMaxCount + Define.PassiveSkillMaxCount;

    #region 초기화
    public SkillSystem(SoDatabase skillDatabase, StagePlayer player)
    {
        _skillDatabase = skillDatabase;
        _player = player;
        _canSelectSkill = true;

        Init();
    }

    private void Init()
    {
        // 딕셔너리 초기화
        _skillDataCache.Clear();
        _maxedSkillIds.Clear();
        _skillDatabase.GetDatabase<SkillData>()
            .ForEach(skillData =>
            {
                if (_skillDataCache.ContainsKey(skillData.Id))
                {
                    Logger.LogWarning("키 중복");
                }
                _skillDataCache[skillData.Id] = skillData;
            });
        _ownedSkills.Clear();
        _combinationRequirementMap.Clear();

        // todo: 기본 스킬 주기
        // ex. 쿠나이
        TrySelectSkill(30);
    }
    #endregion

    #region 스킬 획득
    /// <summary>
    /// [public] 스킬 선택하기
    /// </summary>
    /// <param name="id"></param>
    public bool TrySelectSkill(int id)
    {
        if (!_skillDataCache.TryGetValue(id, out SkillData data))
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
        baseSkill.Init(data);
        UpdateSkillSelectCondition(id);

        return true;
    }

    /// <summary>
    /// 액티브 스킬 획득
    /// </summary>
    /// <returns></returns>
    private ActiveSkill GetActiveSkill()
    {
        _activeSkillCount++;
        return _player.gameObject.AddComponent<ActiveSkill>();
    }

    /// <summary>
    /// 패시브 스킬 획득
    /// </summary>
    /// <returns></returns>
    private PassiveSkill GetPassiveSkill()
    {
        _passiveSkillCount++;
        return _player.gameObject.AddComponent<PassiveSkill>();
    }

    /// <summary>
    /// 조합 스킬 획득
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private ActiveSkill GetCombinationSkill(int id)
    {
        foreach (int combinationId in _skillDataCache[id].CombinationIds)
        {
            if (_skillDataCache[combinationId].Type == SkillType.Active)
            {
                _activeSkillCount--;
                // 액티브 스킬일 경우 삭제
                GameObject.Destroy(_ownedSkills[combinationId].gameObject);
                _ownedSkills.Remove(combinationId);
            }
            else if (_skillDataCache[combinationId].Type == SkillType.Passive)
            {
                _passiveSkillCount--;
            }
        }

        _activeSkillCount++;
        return _player.gameObject.AddComponent<ActiveSkill>();
    }
    #endregion

    #region 스킬 획득 조건 관리
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
                    _maxedSkillIds.Add(id);                         // 만렙 처리
                    Logger.Log($"스킬 잠금(사유: 최대 레벨): {_skillDataCache[id].DisplayName}");
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
                    _maxedSkillIds.Add(id);                         // 만렙 처리
                    Logger.Log($"스킬 잠금(사유: 최대 레벨): {_skillDataCache[id].DisplayName}");
                }
                break;
            case SkillType.Combination:
                if (_combinationRequirementMap.ContainsKey(id))
                {
                    _combinationRequirementMap.Remove(id);
                }
                break;
        }

        CheckCanSelectSkill();
    }

    /// <summary>
    /// 스킬 조합 조건 딕셔너리에 정보 저장
    /// </summary>
    /// <param name="combinationIds"></param>
    private void ApplyCombinationSkillDict(int[] combinationIds)
    {
        if (combinationIds == null || combinationIds.Length == 0) return;

        foreach (int combinationId in combinationIds)
        {
            if (_combinationRequirementMap.ContainsKey(combinationId))
            {
                _combinationRequirementMap[combinationId]++;
                Logger.Log($"조합 스킬 해금: {_skillDataCache[combinationId].DisplayName}");
            }
            else
            {
                _combinationRequirementMap.Add(combinationId, 1);
            }
        }
    }

    private void CheckCanSelectSkill()
    {
        if (_activeSkillCount + _passiveSkillCount < TotalMaxSkillCount) return;
        foreach (BaseSkill ownedSkill in _ownedSkills.Values)
        {
            if (!ownedSkill.IsMaxLevel)
            {
                return;
            }
        }
        _canSelectSkill = false;
    }

    // todo: 조합 스킬 획득 가능한지 한 번 더 확인
    #endregion

    public List<SkillSelectDto> ShowSelectableSkills(int count)
    {
        HashSet<int> selectedSkillId = new();
        List<SkillSelectDto> skillSelectDtos = new();

        // todo: 체력 회복 or 돈 -> UI에 어떻게 표현할지 논의 필요
        if (!_canSelectSkill) return null;

        // 조합 스킬 있는지 확인
        foreach (KeyValuePair<int, int> combinationSkillTerm in _combinationRequirementMap)
        {
            if (combinationSkillTerm.Value == 2)
            {
                SkillData skillData = _skillDataCache[combinationSkillTerm.Key];
                skillSelectDtos.Add(new SkillSelectDto(
                    skillData.Id,
                    0,
                    skillData.DisplayName,
                    skillData.Description,
                    skillData.Icon,
                    skillData.Type,
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

        // 1. 스킬 슬롯 전부 찼을 경우
        // 소유한 스킬 중에 선택할 수 있는 스킬 획득
        if (_activeSkillCount + _passiveSkillCount == TotalMaxSkillCount)
        {
            foreach (int id in _ownedSkills.Keys)
            {
                selectedSkillId.Add(id);
            }
        }
        // 2. 스킬 전부 획득하지 않았을 경우
        // 1) 액티브 스킬만 전부 뽑았을 때
        else if (_activeSkillCount == Define.ActiveSkillMaxCount && _passiveSkillCount != Define.PassiveSkillMaxCount)
        {
            foreach (SkillData data in _skillDataCache.Values)
            {
                if (data.Type == SkillType.Passive)
                {
                    selectedSkillId.Add(data.Id);
                }
            }
        }
        // 2) 패시브 스킬만 전부 뽑았을 때
        else if (_activeSkillCount != Define.ActiveSkillMaxCount && _passiveSkillCount == Define.PassiveSkillMaxCount)
        {
            foreach (SkillData data in _skillDataCache.Values)
            {
                if (data.Type == SkillType.Active)
                {
                    selectedSkillId.Add(data.Id);
                }
            }
        }
        // 3) 전부 다 안 뽑았을 때
        else
        {
            foreach (SkillData data in _skillDataCache.Values)
            {
                if (data.Type != SkillType.Combination)
                {
                    selectedSkillId.Add(data.Id);
                }
            }
        }

        // 만렙 스킬인 경우 제거
        _maxedSkillIds.ForEach(id => selectedSkillId.Remove(id));

        AddRandomSkillDto(skillSelectDtos, selectedSkillId.ToList(), count);

        //// 테스트용
        //skillSelectDtos.Clear();
        //SkillData testSkillData = _skillDataCache[30];
        //BaseSkill baseSkill = _ownedSkills[30];
        //for (int i = 0; i < count; i++)
        //{
        //    skillSelectDtos.Add(new SkillSelectDto(
        //        testSkillData.Id,
        //        baseSkill.CurLevel,
        //        testSkillData.name,
        //        testSkillData.Description,
        //        testSkillData.Icon,
        //        testSkillData.Type,
        //        null));
        //}

        return skillSelectDtos;
    }

    private void AddRandomSkillDto(List<SkillSelectDto> skillSelectDtos, List<int> list, int count)
    {
        int pickCount = count - skillSelectDtos.Count;

        list.Random(pickCount).ForEach(id =>
        {
            // 뽑아야하는 스킬 정보 가져오기
            _skillDataCache.TryGetValue(id, out SkillData skillData);
            _ownedSkills.TryGetValue(id, out BaseSkill skill);

            // 조합 스킬 아이콘 가져오기
            Sprite[] icons = new Sprite[skillData.CombinationIds.Length];
            for (int i = 0; i < skillData.CombinationIds.Length; i++)
            {
                _skillDataCache.TryGetValue(i, out SkillData combinationSkill);
                icons[i] = combinationSkill.Icon;
            }

            // dto 만들기
            skillSelectDtos.Add(new SkillSelectDto(
                skillData.Id,
                skill == null ? 0 : skill.CurLevel,
                skillData.DisplayName,
                skillData.Description,
                skillData.Icon,
                skillData.Type,
                icons));
        });
    }
}
