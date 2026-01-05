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

    #endregion

    #region 초기화
    public virtual void Init(SkillData data)
    {
        skillData = data;
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
        if (CurLevel < Define.SkillMaxLevel)
        {
            CurLevel++;
        }
    }
}
