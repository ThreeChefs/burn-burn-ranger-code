using DG.Tweening;
using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 투사체
/// </summary>
public class PlayerProjectile : BaseProjectile
{
    #region 필드
    [SerializeField] private Transform _aoePivot;

    // 캐싱
    protected Transform player;
    protected ActiveSkill skill;

    // 스텟
    protected PlayerStat projectileSpeed;
    protected PlayerStat projectileRange;

    // 타이머
    protected float tickIntervalTimer;

    // 이동 속도
    protected override float Speed
    {
        get
        {
            float speed = base.Speed * projectileSpeed.MaxValue;
            if (_speedMultiplier != null)
            {
                speed *= _speedMultiplier[skill.CurLevel - 1];
            }
            return speed;
        }
    }
    private float[] _speedMultiplier;

    // 크기
    private float[] _scaleMultipliers;
    private Tween _scaleTween;
    private float _scaleDuration = 1f;
    #endregion

    public virtual void Init(ActiveSkill activeSkill, PoolObjectData originData)
    {
        skill = activeSkill;
        ActiveSkillData data = skill.Data;

        skill.SkillValues.TryGetValue(SkillValueType.ProjectileSpeed, out _speedMultiplier);
        skill.SkillValues.TryGetValue(SkillValueType.Scale, out _scaleMultipliers);

        base.Init(PlayerManager.Instance.Condition[StatType.Attack], originData);
    }

    #region Unity API
    protected override void Start()
    {
        base.Start();

        // 스텟 캐싱
        player = PlayerManager.Instance.StagePlayer.transform;
        PlayerCondition condition = PlayerManager.Instance.Condition;
        projectileSpeed = condition[StatType.ProjectileSpeed];
        projectileRange = condition[StatType.ProjecttileRange];
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (IsHitTarget(layer))
        {
            HandleHit(collision);
        }

        if (IsReflectTarget(layer))
        {
            HandleReflection(collision);
        }
    }
    #endregion

    #region 충돌 처리
    private bool IsHitTarget(int layer)
    {
        return ((1 << layer) & data.TargetLayerMask) != 0;
    }

    private bool IsReflectTarget(int layer)
    {
        return ((1 << layer) & data.ReflectionLayerMask) != 0;
    }

    protected virtual void HandleHit(Collider2D collision)
    {
        switch (data.HitType)
        {
            case ProjectileHitType.Immediate:
                HitContext context = GetHitContext(collision);
                OnValidHit(in context);

                // 관통 무한
                if (passCount == Define.InfinitePass)
                {
                    return;
                }

                passCount--;

                if (passCount <= 0)
                {
                    if (data.HasAreaPhase)  // 장판 존재
                    {
                        UpdateAreaPhase();
                    }

                    gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 특정 콜라이더에 반사 처리
    /// </summary>
    /// <param name="collision"></param>
    private void HandleReflection(Collider2D collision)
    {
        Vector2 norm = Vector2.zero;

        if (collision.gameObject.layer == Define.WallLayer)
        {
            Vector2 hitPos = collision.ClosestPoint(transform.position);
            norm = ((Vector2)transform.position - hitPos).normalized;
        }
        else if (collision.gameObject.layer == Define.MonsterLayer)
        {
            norm = (transform.position - collision.transform.position).normalized;
        }

        if (norm.sqrMagnitude < 0.0001f) return;

        moveDir = Vector2.Reflect(moveDir, norm).normalized;
        transform.position += moveDir * 0.05f;        // 재충돌 방지
    }
    #endregion

    #region Pool Object 관리 - Spawn / OnEnable / OnDisable
    public override void Spawn(Vector2 spawnPos, Transform target)
    {
        base.Spawn(spawnPos, target);
        SetScale();
    }

    public override void Spawn(Vector2 spawnPos, Vector2 dir)
    {
        base.Spawn(spawnPos, dir);
        SetScale();
    }

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();

        if (skill != null)
        {
            skill.OnLevelUp += UpdateScaleTo;
        }
    }

    protected override void OnDisableInternal()
    {
        _scaleTween?.Kill();
        base.OnDisableInternal();

        // 타이머 초기화
        tickIntervalTimer = 0f;
        tickTimer = 0f;

        if (skill != null)
        {
            skill.OnLevelUp -= UpdateScaleTo;
        }
    }
    #endregion

    #region Phase 관리
    protected override void UpdateFlyPhase()
    {
        if (!data.HasAreaPhase || passCount > 0) return;

        flyTimer += Time.deltaTime;
        if (flyTimer < data.AoEData.FlyPhaseDuration) return;

        flyTimer = 0f;
        EnterAreaPhase();
    }

    /// <summary>
    /// 즉발 장판이면 hit 하고 return, 아니면 틱 대미지로 계속 대미지
    /// </summary>
    protected override void UpdateAreaPhase()
    {
        if (!data.HasAreaPhase) return;

        tickTimer += Time.deltaTime;
        if (tickTimer < data.AoEData.TickInterval) return;

        tickTimer = 0f;

        //Logger.Log("장판 켜짐");
        Collider2D[] targets = CheckTargetsAndHit();

        foreach (Collider2D target in targets)
        {
            HitContext context = GetHitContext(target, _aoePivot.position);
            data.AoEData.AreaEffects.ForEach(effect => effect.Apply(in context));
        }

        if (data.AoEData.IsInstant)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Hit Utils
    protected override float CalculateDamage()
    {
        return attack.MaxValue * skill.SkillValues[SkillValueType.AttackPower][skill.CurLevel - 1];
    }

    private void OnValidHit(in HitContext context)
    {
        foreach (BaseEffectSO effect in data.HitEffects)
        {
            effect.Apply(in context);
        }
    }

    private Collider2D[] CheckTargetsAndHit()
    {
        float scaleMultiplier = 1f;
        if (_scaleMultipliers != null)
        {
            scaleMultiplier *= _scaleMultipliers[skill.CurLevel - 1];
        }

        return data.AoEData.AoEShape switch
        {
            AoEShape.Circle => Physics2D.OverlapCircleAll(
                _aoePivot.position,
                data.AoEData.Radius * projectileRange.MaxValue * scaleMultiplier,
                data.AoEData.AoETargetLayer),
            AoEShape.Box => Physics2D.OverlapBoxAll(
                _aoePivot.position,
                data.AoEData.BoxSize * projectileRange.MaxValue * scaleMultiplier,
                360,
                data.AoEData.AoETargetLayer),
            _ => throw new System.NotImplementedException(),
        };
    }

    private HitContext GetHitContext(Collider2D target)
    {
        return new()
        {
            attacker = player,
            damage = CalculateDamage(),
            hitPos = transform.position,
            directTarget = target,
            projectileData = data
        };
    }

    private HitContext GetHitContext(Collider2D target, Vector3 position)
    {
        return new()
        {
            attacker = player,
            damage = CalculateDamage(),
            hitPos = position,
            directTarget = target,
            projectileData = data
        };
    }
    #endregion

    #region Level Value Utils
    private void UpdateScaleTo()
    {
        if (projectileRange == null) return;
        Vector3 scale = Vector3.one * projectileRange.MaxValue;

        // 스킬 시스템
        if (_scaleMultipliers != null)
        {
            scale *= _scaleMultipliers[skill.CurLevel - 1];
        }

        _scaleTween?.Complete();
        _scaleTween = transform.DOScale(scale, _scaleDuration);
    }

    private void SetScale()
    {
        if (projectileRange == null) return;

        Vector3 scale = Vector3.one * projectileRange.MaxValue;
        if (_scaleMultipliers != null)
        {
            scale *= _scaleMultipliers[skill.CurLevel - 1];
        }
        transform.localScale = scale;
    }
    #endregion

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        _aoePivot = transform.FindChild<Transform>("AoEPivot");
        if (_aoePivot == null)
        {
            _aoePivot = new GameObject("AoEPivot").transform;
            Transform model = transform.FindChild<Transform>("Model");
            _aoePivot.SetParent(model.transform);
            _aoePivot.localPosition = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (data == null || data.AoEData == null) return;

        Gizmos.color = Color.red;

        switch (data.AoEData.AoEShape)
        {
            case AoEShape.Circle:
                DrawCircle(_aoePivot.position, data.AoEData.Radius);
                break;

            case AoEShape.Box:
                DrawBox(_aoePivot.position, data.AoEData.BoxSize);
                break;
        }
    }

    private void DrawCircle(Vector3 center, float radius)
    {
        const int SEGMENTS = 32;
        Vector3 prev = center + Vector3.right * radius;

        for (int i = 1; i <= SEGMENTS; i++)
        {
            float angle = i * Mathf.PI * 2f / SEGMENTS;
            Vector3 next = center + new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
    private void DrawBox(Vector3 center, Vector2 size)
    {
        Vector3 half = size * 0.5f;

        Vector3 tl = center + new Vector3(-half.x, half.y);
        Vector3 tr = center + new Vector3(half.x, half.y);
        Vector3 br = center + new Vector3(half.x, -half.y);
        Vector3 bl = center + new Vector3(-half.x, -half.y);

        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(tr, br);
        Gizmos.DrawLine(br, bl);
        Gizmos.DrawLine(bl, tl);
    }
#endif
}
