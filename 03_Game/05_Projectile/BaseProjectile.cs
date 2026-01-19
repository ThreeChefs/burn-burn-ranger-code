using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : PoolObject, IAttackable
{
    #region 필드
    [Header("비주얼")]
    [SerializeField] protected Transform vfxs;

    protected ProjectileData data;

    protected ProjectileMoveType type;
    protected int passCount;

    // 스텟
    protected BaseStat attack;          // 공격

    // 타겟
    [SerializeField] protected Transform target;
    protected Vector3 movePos;
    protected Vector3 moveDir;

    // 이동
    protected virtual float Speed => data.Speed * speedMultiplier;
    protected float speedMultiplier;
    protected float lifeTimer;

    // 유도
    protected float guidanceTimer;

    // 폭발 / 장판
    protected ProjectilePhase phase;
    protected float flyTimer;
    protected float tickTimer;

    // 효과음
    protected bool useCustomSfx;            // sfx를 사용하는 시점을 커스텀
    protected SfxName sfxName;
    protected int sfxIndex;
    protected Coroutine sfxCoroutine;
    private WaitForSeconds _sfxDuration;

    // 카메라
    protected Camera cam;

    // vfx
    private TrailRenderer _trail;
    #endregion

    #region Unity API
    private void Awake()
    {
        if (vfxs != null)
        {
            _trail = vfxs.GetComponentInChildren<TrailRenderer>();
        }
    }

    protected virtual void Start()
    {
        cam = Camera.main;
    }

    protected virtual void Update()
    {
        UpdatePhase();

        if (data.AliveTime < 0) return;

        if (type == ProjectileMoveType.Guidance)
        {
            if (data.GuidanceTime < 0) return;

            guidanceTimer += Time.deltaTime;
            if (guidanceTimer > data.GuidanceTime)
            {
                type = ProjectileMoveType.Straight;
            }
        }

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

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();

        if (_trail != null)
        {
            _trail.enabled = true;
        }
    }

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();

        // 타이머
        lifeTimer = 0f;
        guidanceTimer = 0f;

        // sfx 코루틴
        if (sfxCoroutine != null)
        {
            StopCoroutine(sfxCoroutine);
            sfxCoroutine = null;
        }

        if (_trail != null)
        {
            _trail.Clear();
            _trail.enabled = false;
        }
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

        if (visualData == null)
        {
            Logger.Log($"{data.name}에서 자율적으로 sfx 사용");
            useCustomSfx = true;
            return;
        }

        sfxName = SfxName.Sfx_Projectile;
        sfxIndex = visualData.SfxIndex;

        _sfxDuration = new WaitForSeconds(visualData.SfxInterval);
    }

    public virtual void Spawn(Vector2 spawnPos, Transform target)
    {
        transform.position = spawnPos;
        this.target = target;

        movePos = target.position;
        moveDir = (movePos - transform.position).normalized;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public virtual void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        transform.position = spawnPos;

        moveDir = dir;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
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
        if (type == ProjectileMoveType.Guidance)
        {
            SetGuidance();
        }

        Move();

        if (type == ProjectileMoveType.Reflection)
        {
            HandleScreenReflection();
        }
    }

    protected virtual void Move()
    {
        Vector3 targetPos = Speed * Time.fixedDeltaTime * moveDir;
        transform.position += targetPos;
    }

    protected virtual void SetGuidance()
    {
        if (target == null) return;
        moveDir = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected virtual void HandleScreenReflection()
    {
        if (((1 << Define.WallLayer) & data.ReflectionLayerMask) == 0) return;

        Vector2 pos = transform.position;
        Vector2 dir = moveDir;
        Vector2 camPos = cam.transform.position;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float minX = camPos.x - halfW;
        float maxX = camPos.x + halfW;
        float minY = camPos.y - halfH;
        float maxY = camPos.y + halfH;

        bool reflected = false;

        if (pos.x < minX || pos.x > maxX)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            dir.x *= -1;
            reflected = true;
        }

        if (pos.y < minY || pos.y > maxY)
        {
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            dir.y *= -1;
            reflected = true;
        }

        if (reflected)
        {
            moveDir = dir.normalized;
            transform.position = pos;
            PlaySfxOnce();
        }
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
        speedMultiplier = data.AoEData.IsMoving ? 1f : 0f;
        tickTimer = 0f;
    }

    protected virtual void UpdateAreaPhase()
    {
    }
    #endregion

    #region 사운드
    protected void PlaySfxOnce()
    {
        if (sfxIndex >= 0 && !useCustomSfx && SfxLimiter.CanPlay(sfxName, sfxIndex))
        {
            SoundManager.Instance.PlaySfx(sfxName, idx: sfxIndex);
        }
    }

    protected IEnumerator PlaySfx()
    {
        while (true)
        {
            SoundManager.Instance.PlaySfx(sfxName, idx: sfxIndex);
            yield return _sfxDuration;
        }
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
