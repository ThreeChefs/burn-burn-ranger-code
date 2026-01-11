using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : PoolObject, IAttackable
{
    protected ProjectileData data;

    protected ProjectileMoveType type;
    protected int passCount;

    // 스텟
    protected BaseStat attack;          // 공격

    // 타겟
    [SerializeField] protected Transform target;
    protected Vector3 targetPos;
    protected Vector3 targetDir;
    protected HashSet<Collider2D> targets = new();

    // 이동
    protected virtual float Speed => data.Speed * speedMultiplier;
    protected float speedMultiplier;
    protected float lifeTimer;

    // 폭발 / 장판
    protected ProjectilePhase phase;
    protected float phaseTimer;
    protected float tickTimer;

    #region Unity API
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (data.AliveTime < 0) return;

        lifeTimer += Time.deltaTime;
        if (lifeTimer > data.AliveTime)
        {
            gameObject.SetActive(false);
        }

        UpdatePhase();
    }

    protected virtual void FixedUpdate()
    {
        MoveAndRotate();
    }
    #endregion

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        lifeTimer = 0f;
        targets.Clear();
    }

    #region 초기화
    public virtual void Init(BaseStat attack, ScriptableObject originData)
    {
        this.attack = attack;

        data = originData as ProjectileData;

        type = data.MoveType;
        passCount = data.PassCount;

        speedMultiplier = 1f;

        if (data.HasAreaPhase)
        {
            phase = ProjectilePhase.Fly;
            phaseTimer = 0f;
        }
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
    #endregion

    #region 공격
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & data.TargetLayerMask) == 0) return;

        collision.TryGetComponent<IDamageable>(out var damageable);

        switch (data.HitType)
        {
            case ProjectileHitType.Immediate:
                Attack(damageable);
                if (passCount < 0) return;
                else if (passCount > 0)
                {
                    passCount--;
                    gameObject.SetActive(false);
                    passCount = data.PassCount;
                }
                if (passCount == 0)
                {
                    gameObject.SetActive(false);
                }
                break;
            case ProjectileHitType.Persistent:
            case ProjectileHitType.Timed:
                targets.Add(collision);
                break;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & data.TargetLayerMask) != 0)
        {
            collision.TryGetComponent<IDamageable>(out var damageable);
            targets.Remove(collision);
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
        Vector3 targetPos = Speed * Time.fixedDeltaTime * targetDir;
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

    #region 폭발 / 장판 처리
    private void UpdatePhase()
    {
        switch (phase)
        {
            case ProjectilePhase.Fly:
                UpdateFlyPhase();
                break;
            case ProjectilePhase.Area:
                UpdateAreaPhase();
                break;
        }
    }

    protected virtual void UpdateFlyPhase()
    {
        if (!data.HasAreaPhase) return;
        phaseTimer += Time.deltaTime;
        if (phaseTimer > data.FlyPhaseDuration)
        {
            phaseTimer = 0f;
            EnterAreaPhase();
        }
    }

    protected void EnterAreaPhase()
    {
        Logger.Log("장판모드 들어옴");
        phase = ProjectilePhase.Area;
        speedMultiplier = 0f;
        tickTimer = 0f;
    }

    protected virtual void UpdateAreaPhase()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer > data.TickInterval)
        {
            tickTimer = 0f;
        }
    }
    #endregion

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        var model = transform.FindChild<Transform>("Model");
        if (model == null)
        {
            var newGo = new GameObject("Model");
            newGo.transform.SetParent(transform);
            newGo.AddComponent<SpriteRenderer>();
            newGo.AddComponent<BoxCollider2D>();
        }
    }
#endif
}
