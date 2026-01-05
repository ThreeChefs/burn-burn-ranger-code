using UnityEngine;

public class BaseProjectile : BasePool, IAttackable
{
    protected ActiveSkill skill;
    protected ProjectileType type;
    protected Vector2 offset;
    protected float[] levelValue;
    protected float speed;
    protected int passCount;

    protected PlayerStat attack;

    [SerializeField] protected Transform target;

    #region Unity API
    private void Start()
    {
        attack = PlayerManager.Instance.Condition[StatType.Attack];
    }

    protected virtual void FixedUpdate()
    {
        MoveAndRotate();
    }
    #endregion

    #region 초기화
    public virtual void Init(ActiveSkill skill, ActiveSkillData data)
    {
        this.skill = skill;
        type = data.ProjectileType;
        offset = data.Offset;
        levelValue = data.LevelValue;
        speed = data.Speed;
        passCount = data.PassCount;
    }

    public virtual void Spawn(Vector2 pos)
    {
        target = StageManager.Instance.GetNearestMonster();
        transform.position = pos + offset * (target.position - transform.position).normalized;
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

    protected virtual void MoveAndRotate()
    {
        if (target == null) return;
        Vector2 dir = (target.position - transform.position).normalized;
        Move(dir);
        Rotate(dir);
    }

    protected virtual void Move(Vector2 dir)
    {
    }

    protected virtual void Rotate(Vector2 dir)
    {
    }
}
