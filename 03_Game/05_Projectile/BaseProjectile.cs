using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>
/// 공용으로 사용하는 투사체
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectile : PoolObject, IAttackable, IDamageable
{
    #region 필드
    [Header("비주얼")]
    [SerializeField] protected Transform model;
    [SerializeField] protected Transform vfxs;
    [SerializeField] private bool _useScaleTween;

    protected ProjectileData data;

    protected int passCount;

    // 스텟
    protected BaseStat attack;          // 공격

    // 타겟
    public Transform Target { get; protected set; }
    public Vector3 MoveDir { get; set; }

    // 이동
    protected IProjectileMove move;
    public virtual float Speed => data.Speed * speedMultiplier;
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
    protected WaitForSeconds sfxDuration;

    // 카메라
    protected Camera cam;

    // vfx
    private TrailRenderer _trail;
    private Sequence _scaleSeq;
    private float _scaleTime = 0.4f;

    #endregion

    #region Unity API
    private void Awake()
    {
        if (vfxs != null)
        {
            _trail = GetComponentInChildren<TrailRenderer>();
        }
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        UpdatePhase();
        move?.MoveAndRotate(Time.deltaTime);

        if (data.AliveTime < 0) return;

        lifeTimer += Time.deltaTime;
        if (lifeTimer > data.AliveTime)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void FixedUpdate() { }
    #endregion

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();

        if (_trail != null)
        {
            _trail.Clear();
            _trail.enabled = true;
        }


        if (data != null && _useScaleTween)
        {
            model.localScale = Vector2.zero;
            _scaleSeq = DOTween.Sequence()
                .Append(model.DOScale(1f, _scaleTime))
                .AppendInterval(data.AliveTime - _scaleTime * 2f)
                .Append(model.DOScale(0f, _scaleTime));
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

        if (_useScaleTween)
        {
            _scaleSeq.Kill();
        }
    }

    #region 초기화
    public virtual void Init(BaseStat attack, PoolObjectData originData)
    {
        this.attack = attack;

        data = originData as ProjectileData;

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
            useCustomSfx = true;
            return;
        }

        sfxName = SfxName.Sfx_Projectile;
        sfxIndex = visualData.SfxIndex;

        sfxDuration = new WaitForSeconds(visualData.SfxInterval);
    }

    public virtual void Spawn(Vector2 spawnPos, Transform target)
    {
        transform.position = spawnPos;
        this.Target = target;

        if (target != null)
        {
            Vector3 movePos = target.position;
            MoveDir = (movePos - transform.position).normalized;
            CreateMove();
        }

        PlaySfxOfSpawnType();
    }

    public virtual void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        transform.position = spawnPos;
        MoveDir = dir;
        CreateMove();
        PlaySfxOfSpawnType();
    }

    private void CreateMove()
    {
        IProjectileMove move;
        move = CreateBaseMove();

        if ((data.MoveFeature & ProjectileMoveFeature.Guidance) != 0)
        {
            move = new GudianceMove(this, move, data.GuidanceTime);
        }

        if ((data.MoveFeature & ProjectileMoveFeature.Reflection) != 0)
        {
            move = new ReflectionMove(this, move, data.ReflectionLayerMask);
        }

        this.move = move;
    }

    private IProjectileMove CreateBaseMove()
    {
        return data.BaseMoveType switch
        {
            ProjectileBaseMoveType.Straight => new StraightMove(this),
            _ => null
        };
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

    #region 피격
    public void TakeDamage(float value)
    {
        gameObject.SetActive(false);
        Logger.Log("투사체 제거");
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
        phase = ProjectilePhase.Area;
        speedMultiplier = data.AoEData.IsMoving ? 1f : 0f;
        tickTimer = 0f;
    }

    protected virtual void UpdateAreaPhase()
    {
    }
    #endregion

    #region 사운드
    public void PlaySfxOnce()
    {
        if (sfxIndex >= 0 && !useCustomSfx)
        {
            SoundManager.Instance.PlaySfx(sfxName, idx: sfxIndex);
        }
    }

    protected IEnumerator PlaySfx()
    {
        while (true)
        {
            PlaySfxOnce();
            yield return sfxDuration;
        }
    }

    protected void PlaySfxOfHitType()
    {
        if (data.VisualData != null && data.VisualData.SfxType == ProjectileSfxType.Hit)
        {
            PlaySfxOnce();
        }
    }

    protected void PlaySfxOfExplodeType()
    {
        if (data.VisualData != null && data.VisualData.SfxType == ProjectileSfxType.Explode)
        {
            PlaySfxOnce();
        }
    }

    protected void PlaySfxOfSpawnType()
    {
        if (data.VisualData != null)
        {
            switch (data.VisualData.SfxType)
            {
                case ProjectileSfxType.SpawnOnce:
                    PlaySfxOnce();
                    break;
                case ProjectileSfxType.SpawnLoop:
                    if (sfxCoroutine == null)
                    {
                        sfxCoroutine = StartCoroutine(PlaySfx());
                    }
                    break;
            }
        }
    }

    protected void PlaySfxOfAoEType()
    {
        if (data.VisualData != null
            && data.VisualData.SfxType == ProjectileSfxType.AoE
            && sfxCoroutine == null)
        {
            sfxCoroutine = StartCoroutine(PlaySfx());
        }
    }
    #endregion

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        model = transform.FindChild<Transform>("Model");
        if (model == null)
        {
            model = new GameObject("Model").transform;
            model.SetParent(transform);
            model.gameObject.AddComponent<SpriteRenderer>();
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
