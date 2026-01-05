using UnityEngine;

public class BaseProjectile : BasePool, IAttackable
{
    protected ActiveSkill skill;
    protected ProjectileType type;
    protected Vector2 offset;
    protected float[] levelValue;

    protected PlayerStat attack;

    protected IDamageable target;

    #region Unity API
    private void Start()
    {
        attack = PlayerManager.Instance.Condition[StatType.Attack];
    }
    #endregion

    #region 초기화
    public virtual void Init(ActiveSkill skill, ActiveSkillData data)
    {
        this.skill = skill;
        type = data.ProjectileType;
        offset = data.Offset;
    }

    public virtual void Spawn(Vector2 pos)
    {
        transform.position = pos + offset;
        target = StageManager.Instance.GetNearestMonster();
    }
    #endregion

    #region 공격
    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CalculateDamage());
    }

    protected virtual float CalculateDamage()
    {
        return attack.CurValue * levelValue[skill.CurLevel - 1];
    }
    #endregion
}
