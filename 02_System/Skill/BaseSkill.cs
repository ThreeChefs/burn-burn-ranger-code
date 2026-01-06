using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 스킬 기본 클래스
/// </summary>
public abstract class BaseSkill : MonoBehaviour, IAttackable
{
    #region 필드
    // 스킬 데이터
    [SerializeField, ReadOnly] protected SkillData skillData;
    [field: SerializeField] public int CurLevel { get; protected set; }
    public bool IsMaxLevel { get; private set; }

    #endregion

    #region 초기화
    public virtual void Init(SkillData data)
    {
        skillData = data;
        IsMaxLevel = false;

        Logger.Log($"스킬 획득: {skillData.DisplayName}");

        LevelUp();
    }
    #endregion

    #region Unity API
    protected virtual void Update()
    {
    }

    protected virtual void OnDestroy()
    {
    }
    #endregion

    public virtual void LevelUp()
    {
        if (CurLevel >= Define.SkillMaxLevel) return;

        CurLevel++;

        switch (skillData.Type)
        {
            case SkillType.Active:
            case SkillType.Passive:
                IsMaxLevel = CurLevel == Define.SkillMaxLevel;
                break;
            case SkillType.Combination:
                IsMaxLevel = CurLevel == 1;
                break;
        }

        Logger.Log($"스킬 레벨업: {skillData.DisplayName} / 레벨: {CurLevel} {(IsMaxLevel ? " - 최대 레벨" : "")}");
    }
}
