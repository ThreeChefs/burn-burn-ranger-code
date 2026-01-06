using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
public abstract class BaseProjectile : BasePool, IAttackable
{
    [SerializeField] protected ProjectileData data;

    protected ProjectileType type;
    protected float damageMultiplier;
    protected float speed;
    protected int passCount;

    // 공격 스텟
    protected BaseStat attack;

    [SerializeField] protected Transform target;
    protected LayerMask targetLayer;
    protected Vector3 targetPos;
    protected Vector3 targetDir;

    #region Unity API
    private void Awake()
    {
        type = data.ProjectileType;
        damageMultiplier = data.DamageMultiplier;
        speed = data.Speed;
        passCount = data.PassCount;
    }

    protected virtual void FixedUpdate()
    {
        MoveAndRotate();
    }
    #endregion

    #region 초기화
    public virtual void Init(BaseStat attack)
    {
        this.attack = attack;
    }

    public virtual void Spawn(Vector2 pos)
    {
        targetPos = StageManager.Instance.GetNearestMonster().position;
        transform.position = pos + (Vector2)(targetPos - transform.position).normalized;
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
        return attack.CurValue * damageMultiplier;
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

#if UNITY_EDITOR
    protected virtual void Reset()
    {
    }

    internal abstract void Spawn(Vector2 spawnPos, Transform transform);
#endif
}
