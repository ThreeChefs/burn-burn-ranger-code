using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 선택 및 부여를 관리하는 시스템
/// </summary>
public class SkillSystem
{
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
    private int TotalMaxSkillCount => Define.ActiveSkillMaxCount + Define.PassiveSkillMaxCount;

    // public readonly colliections
    public IReadOnlyDictionary<int, BaseSkill> OwnedSkills => _ownedSkills;

    #region 초기화
    public SkillSystem(SoDatabase skillDatabase, StagePlayer player)
    {
        _player = player;
        _canSelectSkill = true;

        // Collection 초기화
        _ownedSkills.Clear();

        InitializeSkillTable(skillDatabase.GetDatabase<SkillData>());

        AcquireEquipmentSkills();
        EnsureDefaultSkill();
    }

    private void InitializeSkillTable(List<SkillData> skillDatabase)
    {
        // Array 초기화 : skill id를 index로 사용
        int maxId = 0;
        foreach (SkillData skillData in skillDatabase)
        {
            maxId = Math.Max(maxId, skillData.Id);
        }
        _skillTable = new SkillData[maxId + 1];
        _skillStates = new SkillState[maxId + 1];

        foreach (SkillData skillData in skillDatabase)
        {
            if (skillData.Id == _defaultSkillId)
            {
                _defaultSkillData = skillData;
            }

            // 착용하지 않은 무기 스킬 빼기
            if (skillData is ActiveSkillData activeSkillData && activeSkillData.IsWeaponSkill)
            {
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
    }

    /// <summary>
    /// 장비 스킬로 가지고 있는 스킬 획득
    /// </summary>
    private void AcquireEquipmentSkills()
    {
        // skill id, level
        IReadOnlyDictionary<int, int> defaultSkills = PlayerManager.Instance.Equipment.HavingSkills;
        foreach (KeyValuePair<int, int> keyValuePair in defaultSkills)
        {
            SkillData skill = _skillTable[keyValuePair.Key];
            for (int j = 0; j < keyValuePair.Value; j++)
            {
                TryAcquireSkill(skill.Id);
            }
        }
    }

    /// <summary>
    /// 무기 안 가지고 있으면 디폴트 주기
    /// </summary>
    private void EnsureDefaultSkill()
    {
        if (!_hasWeapon)
        {
            Logger.Log("기본 무기 사용");
            _skillTable[_defaultSkillId] = _defaultSkillData;
            _skillStates[_defaultSkillId] |= SkillState.CanDraw;
            TryAcquireSkill(_defaultSkillId);
        }
    }
    #endregion

    #region 스킬 획득
    /// <summary>
    /// [public] 스킬 습득하기
    /// </summary>
    /// <param name="id"></param>
    public bool TryAcquireSkill(int id)
    {
        // 이미 스킬이 있는 경우 레벨업
        if (_ownedSkills.TryGetValue(id, out var skill))
        {
            skill.LevelUp();
            UpdateSkillStatesOnAcquire(id);
            return true;
        }

        SkillData data = _skillTable[id];
        if (data == null) { return false; }

        // 없는 경우 스킬 획득
        BaseSkill baseSkill = (data.Type) switch
        {
            SkillType.Active => AcquireActiveSkill(id),
            SkillType.Combination => AcquireCombinationSkill(id),
            SkillType.Passive => AcquirePassiveSkill(),
            _ => null
        };

        if (baseSkill == null) { return false; }

        // 스킬 획득 후 초기화
        _ownedSkills.Add(id, baseSkill);
        baseSkill.Init(data);
        UpdateSkillStatesOnAcquire(id);

        return true;
    }

    /// <summary>
    /// 액티브 스킬 획득
    /// </summary>
    /// <returns></returns>
    private ActiveSkill AcquireActiveSkill(int id)
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
    private PassiveSkill AcquirePassiveSkill()
    {
        _passiveSkillCount++;
        return _player.gameObject.AddComponent<PassiveSkill>();
    }

    /// <summary>
    /// 조합 스킬 획득
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private ActiveSkill AcquireCombinationSkill(int id)
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

        return AcquireActiveSkill(id);
    }
    #endregion

    #region 스킬 획득 조건 관리
    /// <summary>
    /// id번 스킬 획득 시 스킬 획득 조건을 갱신합니다.
    /// </summary>
    /// <param name="id"></param>
    private void UpdateSkillStatesOnAcquire(int id)
    {
        BaseSkill skill = _ownedSkills[id];
        SkillData data = _skillTable[id];

        // 만렙일 경우 선택 불가
        if (skill.CurLevel == Define.SkillMaxLevel)
        {
            _skillStates[id] &= ~SkillState.CanDraw;
        }

        switch (data.Type)
        {
            case SkillType.Active:
                EvaluateCombinationSkills(data.CombinationIds);
                if (_activeSkillCount == Define.ActiveSkillMaxCount)
                {
                    LockUnownedSkillsByMaxCount(SkillType.Active);
                }
                break;
            case SkillType.Passive:
                EvaluateCombinationSkills(data.CombinationIds);
                if (_passiveSkillCount == Define.PassiveSkillMaxCount)
                {
                    LockUnownedSkillsByMaxCount(SkillType.Passive);
                }
                break;
            case SkillType.Combination:
                _skillStates[id] &= ~SkillState.CombinationReady;
                break;
        }

        LockSelectionSkill();
    }

    /// <summary>
    /// type의 스킬 개수가 최대일 경우 소유하고 있지 않은 스킬 획득 잠금
    /// </summary>
    /// <param name="type"></param>
    private void LockUnownedSkillsByMaxCount(SkillType type)
    {
        for (int id = 0; id < _skillTable.Length; id++)
        {
            // 1. 스킬이 없음
            // 2. 해당 스킬 타입 아님
            // 3. 소유한 스킬
            if (_skillTable[id] == null ||
                _skillTable[id].Type != type ||
                _ownedSkills.ContainsKey(id))
            {
                continue;
            }

            _skillStates[id] &= ~SkillState.CanDraw;
        }
    }

    /// <summary>
    /// 조합 스킬의 아이디를 받고 조건 확인
    /// </summary>
    /// <param name="combinationIds"></param>
    private void EvaluateCombinationSkills(int[] combinationIds)
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

    /// <summary>
    /// 스킬을 전부 획득하고 최대 레벨일 경우 선택 불가능 처리
    /// </summary>
    private void LockSelectionSkill()
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
    #endregion

    public List<SkillSelectDto> GetSelectableSkills(int count)
    {
        if (!_canSelectSkill) return null;

        List<int> selectedSkillId = new();
        List<SkillSelectDto> skillSelectDtos = new(count);

        for (int id = _skillStates.Length - 1; id >= 0; id--)
        {
            // 조합 스킬
            if ((_skillStates[id] & SkillState.CombinationReady) != 0)
            {
                skillSelectDtos.Add(GetSkillSelectDto(_skillTable[id], null));
            }
            else if ((_skillStates[id] & SkillState.CanDraw) != 0)
            {
                selectedSkillId.Add(id);
            }
        }

        AppendRandomSkillDtos(skillSelectDtos, selectedSkillId, count);
        return skillSelectDtos;
    }

    private void AppendRandomSkillDtos(List<SkillSelectDto> skillSelectDtos, List<int> list, int count)
    {
        int pickCount = count - skillSelectDtos.Count;

        list.Random(pickCount).ForEach(id =>
        {
            _ownedSkills.TryGetValue(id, out BaseSkill skill);
            skillSelectDtos.Add(GetSkillSelectDto(_skillTable[id], skill));
        });
    }

    private SkillSelectDto GetSkillSelectDto(SkillData skillData, BaseSkill skill)
    {
        GetLevelAndDescription(skillData, skill, out int curLevel, out string description);
        Sprite[] sprites = new Sprite[0];

        if (skillData.Type == SkillType.Passive)
        {
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
            sprites = icons.ToArray();
        }

        return new SkillSelectDto(
            skillData.Id,
            curLevel,
            skillData.DisplayName,
            description,
            skillData.Icon,
            skillData.Type,
            sprites);
    }

    private void GetLevelAndDescription(
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
