using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 상태 관리
/// </summary>
[Serializable]
public class PlayerCondition
{
    #region 필드
    // 레벨
    public int Level { get; private set; }
    public float CurrentExp { get; private set; }
    public float RequiredExp { get; private set; }

    // 스탯
    private Dictionary<StatType, PlayerStat> _statDict;
    public PlayerStat this[StatType type] => _statDict[type];

    public float CurrentHealth => _statDict[StatType.Health].CurValue;
    public float CurrentStamina => _statDict[StatType.Stamina].CurValue;

    // 이벤트
    public event Action<int> OnLevelChanged;
    public event Action<float> OnExpChanged;
    #endregion

    #region 초기화 & 파괴
    /// <summary>
    /// [생성자] 플레이어 스텟 데이터를 받아 초기화
    /// todo: 추후 json 데이터로 관리 & 플레이어 기본 데이터 주소로 불러오기
    /// </summary>
    /// <param name="data"></param>
    public PlayerCondition(StatData data)
    {
        ConvertStatListToDict(data.Stats);

        // todo: 데이터 연동
        Level = 1;
        CurrentExp = 0f;
        RequiredExp = GetRequiredExp(Level);
    }

    /// <summary>
    /// 스텟 타입과 값을 딕셔너리로 관리하기 위해 초기화
    /// </summary>
    /// <param name="stats"></param>
    /// <exception cref="StatNegativeValueException"></exception>
    private void ConvertStatListToDict(List<StatEntry> stats)
    {
        _statDict = new();

        foreach (StatEntry entry in stats)
        {
            if (entry.BaseValue < 0)
            {
                throw new StatNegativeValueException(entry.StatType, entry.BaseValue);
            }

            _statDict[entry.StatType] = new PlayerStat(entry.BaseValue, entry.StatType);
        }

        // 누락 스텟 확인
        CheckRequiredStat(StatType.Health);
        CheckRequiredStat(StatType.Attack);
        CheckRequiredStat(StatType.Defense);
        CheckRequiredStat(StatType.Speed);
        CheckRequiredStat(StatType.AddEXP);
        CheckRequiredStat(StatType.AddGold);
        CheckRequiredStat(StatType.DropItemRange);
    }

    /// <summary>
    /// 필수 스탯 값이 데이터에 존재하는지 확인
    /// </summary>
    /// <param name="type"></param>
    /// <exception cref="StatMissingKeyException"></exception>
    private void CheckRequiredStat(StatType type)
    {
        if (!_statDict.ContainsKey(type))
        {
            throw new StatMissingKeyException(type);
        }
    }

    /// <summary>
    /// [public] Player가 파괴될 때 수행
    /// </summary>
    public void OnDestroy()
    {
        // 이벤트 초기화
        OnLevelChanged = null;
        OnExpChanged = null;

        foreach (PlayerStat stat in _statDict.Values)
        {
            stat.OnDestroy();
        }
    }
    #endregion

    #region [public] 스텟 관리
    /// <summary>
    /// StatType의 값을 amount 만큼 사용하기
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool TryUse(StatType type, float amount) => _statDict[type].TryUse(amount);

    /// <summary>
    /// [public] 경험치 회득
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        CurrentExp += exp;
        while (CurrentExp > RequiredExp)
        {
            LevelUp();
        }
        OnExpChanged?.Invoke(CurrentExp / RequiredExp);
    }

    /// <summary>
    /// [public] 장비 장착 시 호출
    /// 장비 아이템에서 변경하는 StatType의 value만큼을 적용
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public void OnEquipmentChanged(StatType type, float value)
    {
        _statDict[type].UpdateEquipmentValue(value);
    }
    #endregion

    #region 레벨 관리
    /// <summary>
    /// 레벨업 경험치 받아오기
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private float GetRequiredExp(int level)
    {
        float baseExp = 100f;
        float quad = 20f * level * level;
        float expo = Mathf.Pow(1.05f, level);

        return (baseExp + quad) * expo;
    }

    private void LevelUp()
    {
        CurrentExp -= RequiredExp;
        Level++;
        OnLevelChanged?.Invoke(Level);
        RequiredExp = GetRequiredExp(Level);
    }
    #endregion
}