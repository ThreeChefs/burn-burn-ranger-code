using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 스킬 선택 및 부여를 관리하는 시스템
/// </summary>
public class SkillSystem
{
    private readonly SoDatabase _skillDatabase;
    private readonly StagePlayer _player;

    // 스킬 상태 관리
    private readonly Dictionary<int, BaseSkill> _ownedSkills = new();
    private SkillData[] _skillTable;
    private SkillState[] _skillStates;

    private bool _canSelectSkill;

    private bool _hasWeapon;
    private int _defaultSkillId = 30;
    private SkillData _defaultSkillData;

    // 스킬 조건
    private int _activeSkillCount;
    private int _passiveSkillCount;
    private bool _activeSkillCountRegister;
    private bool _passiveSkillCountRegister;

    private int TotalMaxSkillCount => Define.ActiveSkillMaxCount + Define.PassiveSkillMaxCount;

    private readonly Dictionary<int, int> _combinationRequirementMap = new();
    private readonly List<int> _maxedSkillIds = new();
    private readonly Dictionary<int, SkillData> _skillDataCache = new();

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
        List<SkillData> skillDatabase = _skillDatabase.GetDatabase<SkillData>();

        // Dictionay 초기화
        _ownedSkills.Clear();

        // Array 초기화 - skill id를 index로 사용
        int maxId = 0;
        foreach (SkillData skillData in skillDatabase)
        {
            maxId = Math.Max(maxId, skillData.Id);
        }
        _skillTable = new SkillData[maxId + 1];
        _skillStates = new SkillState[maxId + 1];

        foreach (SkillData skillData in skillDatabase)
        {
            // 디폴트 스킬
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

            _skillTable[skillData.Id] = skillData;
            if (skillData.Type != SkillType.Combination)
            {
                _skillStates[skillData.Id] |= SkillState.CanDraw;
            }
        }

        // skill id, level 
        IReadOnlyDictionary<int, int> defaultSkills = PlayerManager.Instance.Equipment.HavingSkills;
        foreach (KeyValuePair<int, int> keyValuePair in defaultSkills)
        {
            SkillData skill = _skillTable[keyValuePair.Key];
            for (int j = 0; j < keyValuePair.Value; j++)
            {
                TrySelectSkill(skill.Id);
            }
        }

        // 무기 안 가지고 있으면 디폴트 주기
        if (!_hasWeapon)
        {
            Logger.Log("기본 무기 사용");
            _skillTable[_defaultSkillId] = _defaultSkillData;
            _skillStates[_defaultSkillId] |= SkillState.CanDraw;
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
        // 이미 스킬이 있는 경우 레벨업
        if (_ownedSkills.TryGetValue(id, out var skill))
        {
            skill.LevelUp();
            UpdateSkillSelectCondition(id);
            return true;
        }

        SkillData data = _skillTable[id];
        if (data == null) { return false; }

        // 없는 경우 스킬 획득
        BaseSkill baseSkill = (data.Type) switch
        {
            SkillType.Active => GetActiveSkill(id),
            SkillType.Combination => GetCombinationSkill(id),
            SkillType.Passive => GetPassiveSkill(),
            _ => null
        };

        if (baseSkill == null) { return false; }

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
        ActiveSkillData data = _skillTable[id] as ActiveSkillData;
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
        foreach (int combinationId in _skillTable[id].CombinationIds)
        {
            // 액티브 스킬일 경우 삭제
            if (_skillTable[combinationId].Type == SkillType.Active)
            {
                _activeSkillCount--;
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
        SkillData data = _skillTable[id];

        // 만렙일 경우 
        if (skill.CurLevel == Define.SkillMaxLevel)
        {
            _skillStates[id] = SkillState.None;
        }

        switch (data.Type)
        {
            case SkillType.Active:
                ApplyCombinationSkillCondition(data.CombinationIds); // 조합 스킬 조건 확인
                if (_activeSkillCount == Define.ActiveSkillMaxCount && !_activeSkillCountRegister)
                {
                    UpdateMaxCountCondition(SkillType.Active);
                    _activeSkillCountRegister = true;
                }
                break;
            case SkillType.Passive:
                ApplyCombinationSkillCondition(data.CombinationIds); // 조합 스킬 조건 확인
                if (_passiveSkillCount == Define.ActiveSkillMaxCount && !_passiveSkillCountRegister)
                {
                    UpdateMaxCountCondition(SkillType.Passive);
                    _passiveSkillCountRegister = true;
                }
                break;
            case SkillType.Combination:
                _skillStates[id] = SkillState.None;
                break;
        }

        CheckCanSelectSkill();
    }

    /// <summary>
    /// type의 스킬 개수가 최대일 경우 소유하고 있지 않은 스킬 획득 잠금
    /// </summary>
    /// <param name="type"></param>
    private void UpdateMaxCountCondition(SkillType type)
    {
        for (int id = 0; id < _skillTable.Length; id++)
        {
            // 1. 스킬이 없음
            // 2. 해당 스킬 타입 없음
            // 3. 소유한 스킬
            if (_skillTable[id] == null ||
                _skillTable[id].Type != type ||
                _ownedSkills.ContainsKey(id))
            {
                continue;
            }

            _skillStates[id] = SkillState.None;
        }
    }

    /// <summary>
    /// 조합 스킬의 아이디를 받고 조건 확인
    /// </summary>
    /// <param name="combinationIds"></param>
    private void ApplyCombinationSkillCondition(int[] combinationIds)
    {
        foreach (int combinationId in combinationIds)
        {
            if (combinationId >= _skillTable.Length) continue;

            int[] requiredSkillIds = _skillTable[combinationId].CombinationIds;
            bool unlockSkill = true;

            foreach (int id in requiredSkillIds)
            {
                // 스킬 해금 불가 조건
                // 1. 스킬 미소유
                // 2. active 스킬인데 최대 레벨이 아닐 경우
                if (!_ownedSkills.TryGetValue(id, out BaseSkill skill) ||
                    skill.SkillData.Type == SkillType.Active && skill.CurLevel != Define.SkillMaxLevel)
                {
                    unlockSkill = false;
                    break;
                }
            }

            if (unlockSkill)
            {
                _skillStates[combinationId] |= SkillState.CombinationReady;
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
        if (!_canSelectSkill) return null;

        List<int> selectedSkillId = new();
        List<SkillSelectDto> skillSelectDtos = new(count);

        for (int id = _skillStates.Length - 1; id >= 0; id--)
        {
            // 조합 스킬
            if ((_skillStates[id] & SkillState.CombinationReady) != 0)
            {
                SkillData skillData = _skillTable[id];
                GetLevelAndDescription(skillData, null, out int curLevel, out string description);
                skillSelectDtos.Add(new SkillSelectDto(
                    skillData.Id,
                    curLevel,
                    skillData.DisplayName,
                    description,
                    skillData.Icon,
                    skillData.Type,
                    new Sprite[0]));
            }
            else if ((_skillStates[id] & SkillState.CanDraw) != 0)
            {
                selectedSkillId.Add(id);
            }
        }

        AddRandomSkillDto(skillSelectDtos, selectedSkillId.ToList(), count);

        return skillSelectDtos;
    }

    private void AddRandomSkillDto(List<SkillSelectDto> skillSelectDtos, List<int> list, int count)
    {
        int pickCount = count - skillSelectDtos.Count;

        list.Random(pickCount).ForEach(id =>
        {
            // 뽑아야하는 스킬 정보 가져오기
            SkillData skillData = _skillTable[id];
            _ownedSkills.TryGetValue(id, out BaseSkill skill);

            // 조합 스킬 아이콘 가져오기 
            // 패시브 스킬에서만 조합스킬이 되는 액티브 스킬 아이콘이 보임
            List<Sprite> icons = new();
            foreach (int combinationId in skillData.CombinationIds)
            {
                if (combinationId >= _skillTable.Length || _skillTable[combinationId] == null) continue;
                int[] materialIds = _skillTable[combinationId].CombinationIds;      // 재료 아이디
                foreach (int materialId in materialIds)
                {
                    SkillData materialData = _skillTable[materialId];
                    if (materialData == null) continue;
                    if (materialData.Type == SkillType.Active)
                    {
                        icons.Add(materialData.Icon);
                    }
                }
            }

            GetLevelAndDescription(skillData, skill, out int curLevel, out string description);

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

    private static void GetLevelAndDescription(
        SkillData skillData,
        BaseSkill skill,
        out int curLevel,
        out string description)
    {
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
    }
}
