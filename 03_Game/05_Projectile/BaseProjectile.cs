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

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Vector3 targetPos;
    protected Vector3 targetDir;

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
        targetPos = StageManager.Instance.GetNearestMonster().position;
        transform.position = pos + offset * (targetPos - transform.position).normalized;
    }
    #endregion

    #region 공격
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            Attack((IDamageable)collision);
            if (passCount < 0) return;
            passCount--;
            if (passCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }

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
    }

    protected virtual void Move(Vector2 dir)
    {
    }

    protected virtual void Rotate(Vector2 dir)
    {
    }
}
