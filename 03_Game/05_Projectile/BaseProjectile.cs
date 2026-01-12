using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : PoolObject, IAttackable
{
    [Header("비주얼")]
    [SerializeField] protected Transform vfxs;
    protected GameObject trailVfx;

    protected ProjectileData data;

    protected ProjectileMoveType type;
    protected int passCount;

    // 스텟
    protected BaseStat attack;          // 공격

    // 타겟
    [SerializeField] protected Transform target;
    protected Vector3 targetPos;
    protected Vector3 targetDir;

    // 이동
    protected virtual float Speed => data.Speed * speedMultiplier;
    protected float speedMultiplier;
    protected float lifeTimer;

    // 폭발 / 장판
    protected ProjectilePhase phase;
    protected float flyTimer;
    protected float tickTimer;

    #region Unity API
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        UpdatePhase();

        if (data.AliveTime < 0) return;

        lifeTimer += Time.deltaTime;
        if (lifeTimer > data.AliveTime)
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
        lifeTimer = 0f;

        trailVfx?.SetActive(false);
    }

    #region 초기화
    public virtual void Init(BaseStat attack, PoolObjectData originData)
    {
        this.attack = attack;

        data = originData as ProjectileData;

        type = data.MoveType;
        passCount = data.PassCount;

        speedMultiplier = 1f;

        if (data.HasAreaPhase)
        {
            phase = ProjectilePhase.Fly;
            flyTimer = 0f;
        }

        InitVisualData();
    }

    private void InitVisualData()
    {
        // 비주얼
        ProjectileVisualData visualData = data.VisualData;
        if (visualData != null)
        {
            if (visualData.TrailVfxPrefab != null)
            {
                trailVfx = Instantiate(visualData.TrailVfxPrefab);
                trailVfx.transform.SetParent(vfxs);
            }
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

    public virtual void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        transform.position = spawnPos;

        targetDir = dir;
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
            default:
                break;
        }
    }

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(CalculateDamage());
    }

    protected virtual float CalculateDamage()
    {
        return attack.MaxValue;
    }
    #endregion

    #region 움직임
    private void MoveAndRotate()
    {
        switch (type)
        {
            case ProjectileMoveType.Guidance:
                GuidanceMove();
                GuidanceRotate();
                break;
            default:
                MoveDefault();
                break;
        }
    }

    protected virtual void MoveDefault()
    {
        Vector3 targetPos = Speed * Time.fixedDeltaTime * targetDir;
        transform.position += targetPos;
    }

    protected virtual void GuidanceMove()
    {
    }

    protected virtual void GuidanceRotate()
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
        flyTimer += Time.deltaTime;
        if (flyTimer > data.AoEData.FlyPhaseDuration)
        {
            flyTimer = 0f;
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
    }
    #endregion

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        var model = transform.FindChild<Transform>("Model");
        if (model == null)
        {
            var newGo = new GameObject("Model");
            newGo.transform.SetParent(transform);
            newGo.AddComponent<SpriteRenderer>();
            BoxCollider2D collider2D = newGo.AddComponent<BoxCollider2D>();
            collider2D.isTrigger = true;
        }
        else
        {
            if (model.TryGetComponent<Collider2D>(out var collider2D))
            {
                collider2D.isTrigger = true;
            }
            else
            {
                collider2D = model.AddComponent<BoxCollider2D>();
                collider2D.isTrigger = true;
            }
        }

        // 비주얼
        vfxs = transform.FindChild<Transform>("Vfxs");
        if (vfxs == null)
        {
            vfxs = new GameObject("Vfxs").transform;
            vfxs.SetParent(transform);
        }
    }
#endif
}
