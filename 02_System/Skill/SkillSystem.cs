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
    private int _minCombinationId = int.MaxValue;

    private bool _hasWeapon;
    private int _defaultSkillId = 30;
    private SkillData _defaultSkillData;

    // public readonly colliections
    public IReadOnlyDictionary<int, BaseSkill> OwnedSkills => _ownedSkills;
    public IReadOnlyDictionary<int, int> CombinationRequirementMap => _combinationRequirementMap;
    public IReadOnlyList<int> MaxedSkillIds => _maxedSkillIds;
    public IReadOnlyDictionary<int, SkillData> SkillDataCache => _skillDataCache;

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
        List<SkillData> skillDatabase = _skillDatabase.GetDatabase<SkillData>();

        foreach (SkillData skillData in skillDatabase)
        {
            if (skillData.Id == _defaultSkillId)
            {
                _defaultSkillData = skillData;
            }

            if (skillData is ActiveSkillData activeSkillData && activeSkillData.IsWeaponSkill)
            {
                // 무기 스킬 빼기 (디폴트는 제외)
                if (!PlayerManager.Instance.Equipment.HavingSkills.ContainsKey(activeSkillData.Id))
                {
                    Logger.Log($"장비 안한 무기 스킬: {activeSkillData.Id}");
                    continue;
                }
                _hasWeapon = true;
            }

            if (_skillDataCache.ContainsKey(skillData.Id))
            {
                Logger.LogWarning("키 중복");
            }
            _skillDataCache[skillData.Id] = skillData;

            if (skillData.Type == SkillType.Combination)
            {
                _minCombinationId = Mathf.Min(_minCombinationId, skillData.Id);
            }
        }

        _ownedSkills.Clear();
        _combinationRequirementMap.Clear();

        // todo: 테스트용이니까 지우기
        Dictionary<int, int> testSkills = PlayerManager.Instance.Inventory.RequiredSkills;
        foreach (var keyValuePair in testSkills)
        {
            SkillData skill = _skillDataCache[keyValuePair.Key];
            for (int j = 0; j < keyValuePair.Value; j++)
            {
                TrySelectSkill(skill.Id);
            }
        }

        // skill id, level 
        IReadOnlyDictionary<int, int> defaultSkills = PlayerManager.Instance.Equipment.HavingSkills;
        foreach (KeyValuePair<int, int> keyValuePair in defaultSkills)
        {
            SkillData skill = _skillDataCache[keyValuePair.Key];
            for (int j = 0; j < keyValuePair.Value; j++)
            {
                TrySelectSkill(skill.Id);
            }
        }

        // 무기 안 가지고 있으면 디폴트 주기
        if (!_hasWeapon)
        {
            Logger.Log("기본 무기 사용");
            _skillDataCache.Add(_defaultSkillId, _defaultSkillData);
            TrySelectSkill(_defaultSkillId);
        }
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
            SkillType.Active => GetActiveSkill(id),
            SkillType.Combination => GetCombinationSkill(id),
            SkillType.Passive => GetPassiveSkill(),
            _ => null
        };

        if (baseSkill == null)
        {
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
    private ActiveSkill GetActiveSkill(int id)
    {
        _activeSkillCount++;
        ActiveSkillData data = _skillDataCache[id] as ActiveSkillData;
        ActiveSkill activeSkill = GameObject.Instantiate(data.ActiveSkillPrefab);
        activeSkill.transform.SetParent(PlayerManager.Instance.StagePlayer.SkillContainer);
        activeSkill.transform.localPosition = Vector3.zero;
        return activeSkill;
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
        }

        return GetActiveSkill(id);
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
                Logger.Log($"조합 스킬 해금: {_skillDataCache[combinationId]?.DisplayName}");
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

        if (!_canSelectSkill) return null;

        // 조합 스킬 있는지 확인
        foreach (KeyValuePair<int, int> combinationSkillTerm in _combinationRequirementMap)
        {
            if (combinationSkillTerm.Value == 2)
            {
                SkillData skillData = _skillDataCache[combinationSkillTerm.Key];
                string description;
                if (skillData.Descriptions == null || skillData.Descriptions.Length == 0)
                {
                    description = "설명 쓰세요!!!!!!!!!!!!!";
                }
                else
                {
                    description = skillData.Descriptions[0];
                }
                skillSelectDtos.Add(new SkillSelectDto(
                    skillData.Id,
                    0,
                    skillData.DisplayName,
                    description,
                    skillData.Icon,
                    skillData.Type,
                    new Sprite[0]));
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
                if (id >= _minCombinationId) continue;      // 조합 스킬일 경우 제외 (위에서 처리)
                selectedSkillId.Add(id);
            }
        }
        // 2. 스킬 전부 획득하지 않았을 경우
        // 1) 액티브 스킬을 전부 뽑았을 때
        else if (_activeSkillCount == Define.ActiveSkillMaxCount && _passiveSkillCount != Define.PassiveSkillMaxCount)
        {
            foreach (SkillData data in _skillDataCache.Values)
            {
                // 패시브일 경우 그냥 뽑기
                if (data.Type == SkillType.Passive)
                {
                    selectedSkillId.Add(data.Id);
                }
                // 액티브일 경우 보유한 스킬 중 최대 레벨이 아닌 걸 뽑기
                else if (data.Type == SkillType.Active)
                {
                    if (!_ownedSkills.TryGetValue(data.Id, out BaseSkill skill) ||
                        skill.CurLevel == Define.SkillMaxLevel) continue;
                    selectedSkillId.Add(data.Id);
                }
            }
        }
        // 2) 패시브 스킬만 전부 뽑았을 때
        else if (_activeSkillCount != Define.ActiveSkillMaxCount && _passiveSkillCount == Define.PassiveSkillMaxCount)
        {
            foreach (SkillData data in _skillDataCache.Values)
            {
                // 액티브일 경우 그냥 뽑기
                if (data.Type == SkillType.Active)
                {
                    selectedSkillId.Add(data.Id);
                }
                // 패시브일 경우 보유한 스킬 중 최대 레벨이 아닌 걸 뽑기
                else if (data.Type == SkillType.Passive)
                {
                    if (!_ownedSkills.TryGetValue(data.Id, out BaseSkill skill) ||
                        skill.CurLevel == Define.SkillMaxLevel) continue;
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
            // 패시브 스킬에서만 조합스킬이 되는 액티브 스킬 아이콘이 보임
            List<Sprite> icons = new();
            foreach (int combinationId in skillData.CombinationIds)
            {
                if (!_skillDataCache.ContainsKey(combinationId)) continue;
                int[] materialIds = _skillDataCache[combinationId].CombinationIds;      // 재료 아이디
                foreach (int materialId in materialIds)
                {
                    if (!_skillDataCache.TryGetValue(materialId, out SkillData materialData)) continue;
                    if (materialData.Type == SkillType.Active)
                    {
                        icons.Add(materialData.Icon);
                    }
                }
            }

            int curLevel;
            string description;

            if (skill == null)
            {
                curLevel = 0;
                if (skillData.Descriptions == null || skillData.Descriptions.Length == 0)
                {
                    description = "설명 쓰세요!!!!!!!!!!!!!";
                }
                else
                {
                    description = skillData.Descriptions[0];
                }
            }
            else
            {
                curLevel = skill.CurLevel;
                if (skillData.Descriptions == null || skillData.Descriptions.Length == 0)
                {
                    description = "설명 쓰세요!!!!!!!!!!!!!";
                }
                else
                {
                    description = skillData.Descriptions[skillData.Type == SkillType.Active ? skill.CurLevel : 0];
                }
            }

            // dto 만들기
            skillSelectDtos.Add(new SkillSelectDto(
                skillData.Id,
                curLevel,
                skillData.DisplayName,
                description,
                skillData.Icon,
                skillData.Type,
                skillData.Type == SkillType.Passive ? icons.ToArray() : new Sprite[0]));
        });
    }
}
