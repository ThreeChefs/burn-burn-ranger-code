using UnityEngine;

/// <summary>
/// 스킬 기본 클래스
/// </summary>
public abstract class BaseSkill : MonoBehaviour, IAttackable
{
    #region 필드
    // 스킬 데이터
    protected SkillData skillData;
    protected int skillLevel;

    #endregion

    #region 초기화
    public virtual void Init(SkillData data)
    {
        skillData = data;
    }
    #endregion

    protected virtual void Update()
    {
    }

    public void LevelUp()
    {
        if (skillLevel < Define.SkillMaxLevel)
        {
            skillLevel++;
        }
    }
}
