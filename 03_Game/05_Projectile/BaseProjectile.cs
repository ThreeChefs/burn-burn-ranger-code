using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
public class BaseProjectile : PoolObject, IAttackable
{
    protected ProjectileData data;

    protected ProjectileType type;
    protected int passCount;

    // 공격 스텟
    protected BaseStat attack;

    [SerializeField] protected Transform target;
    protected LayerMask targetLayer;
    protected Vector3 targetPos;
    protected Vector3 targetDir;

    #region Unity API
    protected virtual void FixedUpdate()
    {
        MoveAndRotate();
    }
    #endregion

    // 초기화
    public virtual void Init(BaseStat attack, ProjectileData data)
    {
        this.attack = attack;

        this.data = data;
        type = data.ProjectileType;
        passCount = data.PassCount;
    }

    public virtual void Spawn(Vector2 spawnPos, Transform target)
    {
        transform.position = spawnPos;
        this.target = target;

        targetPos = target.position;
        targetDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

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
        return attack.CurValue * data.DamageMultiplier;
    }
    #endregion

    // 움직임
    private void MoveAndRotate()
    {
        switch (type)
        {
            case ProjectileType.Chase:
                ChaseMove();
                break;
            case ProjectileType.Hover:
                HoverMove();
                HoverRotate();
                break;
            case ProjectileType.Guidance:
                GuidanceMove();
                GuidanceRotate();
                break;
            case ProjectileType.Reflection:
                ReflectionMove();
                ReflectionRotate();
                break;
        }
    }

    #region 탄환 타입 - Chase (단일 추격)
    protected virtual void ChaseMove()
    {
        Vector3 targetPos = data.Speed * Time.fixedDeltaTime * targetDir;
        transform.position += targetPos;
    }
    #endregion

    #region 탄환 타입 - Hover (주위)
    protected virtual void HoverMove()
    {
    }

    protected virtual void HoverRotate()
    {
    }
    #endregion

    #region 탄환 타입 - Guidance (유도 추격)
    protected virtual void GuidanceMove()
    {
    }

    protected virtual void GuidanceRotate()
    {
    }
    #endregion

    #region 탄환 타입 - Reflection (반사)
    protected virtual void ReflectionMove()
    {
    }

    protected virtual void ReflectionRotate()
    {
    }
    #endregion

#if UNITY_EDITOR
    protected virtual void Reset()
    {
    }


#endif
}
