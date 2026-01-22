using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 기본 클래스
/// </summary>
public abstract class BaseSkill : MonoBehaviour, IAttackable
{
    #region 필드
    // 스킬 데이터
    [field: SerializeField, ReadOnly] public SkillData SkillData { get; protected set; }
    [field: SerializeField] public int CurLevel { get; protected set; }
    public bool IsMaxLevel { get; private set; }

    protected Dictionary<SkillValueType, float[]> skillValues = new();
    public IReadOnlyDictionary<SkillValueType, float[]> SkillValues => skillValues;

    // 액션
    public event Action OnLevelUp;
    #endregion

    #region 초기화
    public virtual void Init(SkillData data)
    {
        SkillData = data;
        IsMaxLevel = false;

        foreach (SkillLevelValueEntry entry in data.LevelValues)
        {
            skillValues.Add(entry.SkillValueType, entry.Values);
        }

        LevelUp();
    }
    #endregion

    #region Unity API
    protected virtual void Update()
    {
    }

    protected virtual void OnDestroy()
    {
        OnLevelUp = null;
    }
    #endregion

    public virtual void LevelUp()
    {
        if (CurLevel >= Define.SkillMaxLevel) return;

        switch (SkillData.Type)
        {
            case SkillType.Active:
            case SkillType.Passive:
                CurLevel = Math.Min(CurLevel + 1, Define.SkillMaxLevel);
                IsMaxLevel = CurLevel == Define.SkillMaxLevel;
                break;
            case SkillType.Combination:
                CurLevel = Math.Min(CurLevel + 1, 1);
                IsMaxLevel = CurLevel == 1;
                break;
        }

        OnLevelUp?.Invoke();
    }
}
