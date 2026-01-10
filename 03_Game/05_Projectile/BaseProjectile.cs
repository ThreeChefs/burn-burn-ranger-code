using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
public class BaseProjectile : PoolObject, IAttackable
{
    protected ProjectileData data;

    protected ProjectileMoveType type;
    protected int passCount;

    // 공격 스텟
    protected BaseStat attack;

    [SerializeField] protected Transform target;
    protected Vector3 targetPos;
    protected Vector3 targetDir;

    protected float timer;

    #region Unity API
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (data.AliveTime < 0) return;
        timer += Time.deltaTime;
        if (timer > data.AliveTime)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void FixedUpdate()
    {
        MoveAndRotate();
    }
    #endregion

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        timer = 0f;
    }

    // 초기화
    public virtual void Init(BaseStat attack, ScriptableObject originData)
    {
        this.attack = attack;

        data = originData as ProjectileData;
        type = data.MoveType;
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
        if (((1 << collision.gameObject.layer) & data.TargetLayerMask) != 0)
        {
            collision.TryGetComponent<IDamageable>(out var damageable);
            Attack(damageable);
            if (passCount < 0) return;
            passCount--;
            if (passCount == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CalculateDamage());
    }

    protected virtual float CalculateDamage()
    {
        return attack.MaxValue * data.DamageMultiplier;
    }
    #endregion

    // 움직임
    private void MoveAndRotate()
    {
        switch (type)
        {
            case ProjectileMoveType.Straight:
                ChaseMove();
                break;
            case ProjectileMoveType.Guidance:
                GuidanceMove();
                GuidanceRotate();
                break;
            case ProjectileMoveType.Reflection:
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
