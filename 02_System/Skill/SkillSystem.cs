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

    private bool _hasWeapon;
    private string _defaultSkillName = "쿠나이";
    private SkillData _defaultSkillData;

    // 스킬 조건
    private int _activeSkillCount;
    private int _passiveSkillCount;

    // public readonly colliections
    public IReadOnlyDictionary<int, BaseSkill> OwnedSkills => _ownedSkills;

    #region 초기화
    public SkillSystem(SoDatabase skillDatabase, StagePlayer player)
    {
        _player = player;

        // Collection 초기화
        _ownedSkills.Clear();

        InitializeSkillTable(skillDatabase.GetDatabase<SkillData>());

        AcquireEquipmentSkills();
        EnsureDefaultSkill();
    }

    private void InitializeSkillTable(List<SkillData> skillDatabase)
    {
        int count = skillDatabase.Count;
        _skillTable = new SkillData[count];
        _skillStates = new SkillState[count];

        for (int i = 0; i < skillDatabase.Count; i++)
        {
            _skillTable[i] = skillDatabase[i];
            _skillTable[i].RuntimeIndex = i;

            if (_skillTable[i].DisplayName.Equals(_defaultSkillName))
            {
                _defaultSkillData = _skillTable[i];
            }

            // 착용하지 않은 무기 스킬 뽑기 안됨
            if (_skillTable[i] is ActiveSkillData activeSkillData && activeSkillData.IsWeaponSkill)
            {
                if (!PlayerManager.Instance.Equipment.HavingSkills.ContainsKey(activeSkillData))
                {
                    Logger.Log($"장비 안한 무기 스킬: {activeSkillData.DisplayName}");
                    continue;
                }
                _hasWeapon = true;
            }

            if (_skillTable[i].Type != SkillType.Combination)
            {
                _skillStates[i] |= SkillState.CanDraw;
            }
        }
    }

    /// <summary>
    /// 장비 스킬로 가지고 있는 스킬 획득
    /// </summary>
    private void AcquireEquipmentSkills()
    {
        // skill id, level
        IReadOnlyDictionary<SkillData, int> defaultSkills = PlayerManager.Instance.Equipment.HavingSkills;
        foreach (KeyValuePair<SkillData, int> pair in defaultSkills)
        {
            for (int j = 0; j < pair.Value; j++)
            {
                TryAcquireSkill(pair.Key.RuntimeIndex);
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
            int index = _defaultSkillData.RuntimeIndex;
            _skillStates[index] |= SkillState.CanDraw;
            TryAcquireSkill(index);
        }
    }
    #endregion

    #region 스킬 획득
    /// <summary>
    /// [public] 스킬 습득하기
    /// </summary>
    /// <param name="data"></param>
    public bool TryAcquireSkill(int index)
    {
        SkillData data = _skillTable[index];

        // 이미 스킬이 있는 경우 레벨업
        if (_ownedSkills.TryGetValue(index, out var skill))
        {
            skill.LevelUp();
            if (skill.CurLevel == Define.SkillMaxLevel)
            {
                _skillStates[index] &= ~SkillState.CanDraw;
                _skillStates[index] |= SkillState.LockedByMax;
            }
            UpdateSkillStatesOnAcquire(data);
            return true;
        }

        // 없는 경우 스킬 획득
        BaseSkill baseSkill = (data.Type) switch
        {
            SkillType.Active => AcquireActiveSkill(data),
            SkillType.Combination => AcquireCombinationSkill(data),
            SkillType.Passive => AcquirePassiveSkill(),
            _ => null
        };

        if (baseSkill == null) { return false; }

        // 스킬 획득 후 초기화
        _ownedSkills.Add(index, baseSkill);
        baseSkill.Init(data);
        UpdateSkillStatesOnAcquire(data);

        return true;
    }

    /// <summary>
    /// 액티브 스킬 획득
    /// </summary>
    /// <returns></returns>
    private ActiveSkill AcquireActiveSkill(SkillData data)
    {
        _activeSkillCount++;
        ActiveSkillData activeSkillData = data as ActiveSkillData;
        ActiveSkill activeSkill = GameObject.Instantiate(activeSkillData.ActiveSkillPrefab);
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
    private ActiveSkill AcquireCombinationSkill(SkillData data)
    {
        foreach (SkillData material in data.CombinationSkills)
        {
            // 액티브 스킬일 경우 삭제
            if (material.Type == SkillType.Active)
            {
                _activeSkillCount--;
                GameObject.Destroy(_ownedSkills[material.RuntimeIndex].gameObject);
                _ownedSkills.Remove(material.RuntimeIndex);
            }
        }

        return AcquireActiveSkill(data);
    }
    #endregion

    #region 스킬 획득 조건 관리
    /// <summary>
    /// id번 스킬 획득 시 스킬 획득 조건을 갱신합니다.
    /// </summary>
    /// <param name="id"></param>
    private void UpdateSkillStatesOnAcquire(SkillData data)
    {
        switch (data.Type)
        {
            case SkillType.Active:
                EvaluateCombinationSkills(data.CombinationSkills);
                LockUnownedSkillsByMaxCount(SkillType.Active, _activeSkillCount == Define.ActiveSkillMaxCount);
                break;
            case SkillType.Passive:
                EvaluateCombinationSkills(data.CombinationSkills);
                LockUnownedSkillsByMaxCount(SkillType.Passive, _passiveSkillCount == Define.PassiveSkillMaxCount);
                break;
            case SkillType.Combination:
                _skillStates[data.RuntimeIndex] &= ~SkillState.CombinationReady;
                LockUnownedSkillsByMaxCount(SkillType.Active, _activeSkillCount == Define.ActiveSkillMaxCount);
                break;
        }
    }

    /// <summary>
    /// 조건에 따라 type의 스킬 획득 조절
    /// </summary>
    /// <param name="type"></param>
    private void LockUnownedSkillsByMaxCount(SkillType type, bool isLock)
    {
        for (int index = 0; index < _skillTable.Length; index++)
        {
            // 1. 해당 스킬 타입 아님
            // 2. 소유한 스킬
            if (_skillTable[index].Type != type ||
                _ownedSkills.ContainsKey(index))
            {
                continue;
            }

            if (isLock)
            {
                _skillStates[index] &= ~SkillState.CanDraw;
                _skillStates[index] |= SkillState.LockedByCount;
            }
            else
            {
                _skillStates[index] |= SkillState.CanDraw;
                _skillStates[index] &= ~SkillState.LockedByCount;
            }
        }
    }

    /// <summary>
    /// 조합 스킬의 아이디를 받고 조건 확인
    /// </summary>
    /// <param name="combinationSkills"></param>
    private void EvaluateCombinationSkills(SkillData[] combinationSkills)
    {
        foreach (SkillData combinationSkill in combinationSkills)
        {
            SkillData[] requiredSkills = combinationSkill.CombinationSkills;
            bool unlockSkill = true;

            foreach (SkillData material in requiredSkills)
            {
                int index = material.RuntimeIndex;

                // 스킬 해금 불가 조건
                // 1. 스킬 미소유
                // 2. active 스킬인데 최대 레벨이 아닐 경우
                if (!_ownedSkills.TryGetValue(index, out BaseSkill skill) ||
                    skill.SkillData.Type == SkillType.Active && (_skillStates[index] & SkillState.LockedByMax) == 0)
                {
                    unlockSkill = false;
                    break;
                }
            }

            if (unlockSkill)
            {
                _skillStates[combinationSkill.RuntimeIndex] |= SkillState.CombinationReady;
            }
        }
    }
    #endregion

    #region UI 전달
    public List<SkillSelectDto> GetSelectableSkills(int count)
    {
        List<SkillData> selectedSkills = new();
        List<SkillSelectDto> skillSelectDtos = new();

        for (int id = _skillStates.Length - 1; id >= 0; id--)
        {
            SkillState state = _skillStates[id];
            if ((state & SkillState.LockedByMax) != 0) { continue; }

            // 조합 스킬
            if ((state & SkillState.CombinationReady) != 0)
            {
                skillSelectDtos.Add(GetSkillSelectDto(_skillTable[id], null));
            }
            else if ((state & SkillState.CanDraw) != 0)
            {
                selectedSkills.Add(_skillTable[id]);
            }
        }

        AppendRandomSkillDtos(skillSelectDtos, selectedSkills, count);
        return skillSelectDtos;
    }

    // 5개 뽑을거야! 했는데 실제로 뽑을 수 있는건 5개보다 적을 때를 위해 actualCount 반환
    public List<SkillSelectDto> GetRolledSkills(int count, out int actualCount) 
    {
        actualCount = count;

        if (count <= 0) return null;

        System.Random rnd = Define.Random;

        // 뽑기 가능한 애들 담아둘 리스트
        // ex) 쿠나이1렙, 드론 1렙이면 쿠나이 4개, 드론 4개 들어감
        List<SkillSelectDto> rollableSkill = new();

        // 16칸 짜리! 실제로 뽑을 애들이 앞에 있고, 그 뒤는 rollableSkill 에서 셔플 후 그릴 애들만.
        int maxSlotCount = Define.FortuneBoxSkillSlotCount;
        List<SkillSelectDto> targetSkill = new(maxSlotCount);


        // 조합 스킬부터 넣기!!
        for (int id = _skillStates.Length - 1; id >= 0; id--)
        {
            if ((_skillStates[id] & SkillState.CombinationReady) != 0)
            {
                targetSkill.Add(GetSkillSelectDto(_skillTable[id], null));
            }
        }

        // ownedSkill 에서 레벨업 할 수 있는 횟수만큼 넣어두기
        foreach (BaseSkill ownedSkill in _ownedSkills.Values)
        {
            for (int j = 0; j < Define.SkillMaxLevel - ownedSkill.CurLevel; ++j)
            {
                rollableSkill.Add(GetSkillSelectDto(ownedSkill.SkillData, ownedSkill));
            }

        }

        if (rollableSkill.Count == 0 && targetSkill.Count == 0)
        {
            // 더 뽑을 수 있는 스킬이 없는 상태라면 
            actualCount = 0;
            return null;    // 반환 받은 곳에서는 null 체크하고 고기, 골드 배치하기
        }
        else if (rollableSkill.Count + targetSkill.Count < count)
        {
            // 뽑을 수 있는 스킬 횟수가 요청한 횟수보다 적을 때
            actualCount = rollableSkill.Count + targetSkill.Count;
        }


        if (rollableSkill.Count <= 0)
        {
            // 뽑을 수 있는 스킬 횟수는 0일 때
            // 이미 들어있는 targetSkill 만 반복해서 16칸 채워서 넣어주기 
            int index = 0;
            while (targetSkill.Count < maxSlotCount)
            {
                targetSkill.Add(targetSkill[index]);
                index++;
                if (index >= targetSkill.Count) index = 0;
            }
        }
        else
        {
            // 뽑을 수 있는 일반 스킬들 섞기
            rollableSkill.Shuffle();
            int rollableIndex = 0;

            while (targetSkill.Count < maxSlotCount)
            {
                targetSkill.Add(rollableSkill[rollableIndex]);
                rollableIndex++;
                if (rollableIndex >= rollableSkill.Count) rollableIndex = 0;
            }
        }

        return targetSkill;
    }

    private void AppendRandomSkillDtos(List<SkillSelectDto> skillSelectDtos, List<SkillData> list, int count)
    {
        int pickCount = count - skillSelectDtos.Count;

        list.Random(pickCount).ForEach(data =>
        {
            _ownedSkills.TryGetValue(data.RuntimeIndex, out BaseSkill skill);
            skillSelectDtos.Add(GetSkillSelectDto(_skillTable[data.RuntimeIndex], skill));
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
            foreach (SkillData combinationSkill in skillData.CombinationSkills)
            {
                SkillData[] materials = combinationSkill.CombinationSkills;      // 재료 아이디
                foreach (SkillData material in materials)
                {
                    if (material.Type == SkillType.Active)
                    {
                        icons.Add(material.Icon);
                    }
                }
            }
            sprites = icons.ToArray();
        }

        return new SkillSelectDto(
            skillData.RuntimeIndex,
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
        if (skillData.Descriptions == null || skillData.Descriptions.Length == 0)
        {
            curLevel = 0;
            description = "설명 쓰세요!!!!!!!!!!!!!";
        }
        else
        {
            if (skill == null)
            {
                curLevel = 0;
                description = skillData.Descriptions[0];
            }
            else
            {
                curLevel = skill.CurLevel;
                description = skillData.Descriptions[skillData.Type == SkillType.Active ? skill.CurLevel : 0];
            }
        }
    }
    #endregion
}
