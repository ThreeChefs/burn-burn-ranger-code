using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 투사체
/// </summary>
public class PlayerProjectile : BaseProjectile
{
    // 캐싱
    protected Transform player;
    protected float[] levelValue;

    // 스텟
    protected PlayerStat attackCooldown;
    protected PlayerStat projectileSpeed;

    // 타이머
    protected float tickIntervalTimer;

    protected override float Speed => base.Speed * projectileSpeed.MaxValue;

    public override void Init(BaseStat attack, ScriptableObject originData)
    {
        ActiveSkillData data = originData as ActiveSkillData;
        levelValue = data.LevelValue;

        base.Init(attack, data.ProjectileData);
    }

    #region Unity API
    protected override void Start()
    {
        base.Start();

        // 스텟 캐싱
        player = PlayerManager.Instance.StagePlayer.transform;
        PlayerCondition condition = PlayerManager.Instance.Condition;
        attackCooldown = condition[StatType.AttackCooldown];
        projectileSpeed = condition[StatType.ProjectileSpeed];
    }
    #endregion

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        tickIntervalTimer = 0f;
    }

    private void OnValidHit(in HitContext context)
    {
        foreach (BaseEffectSO effect in data.HitEffects)
        {
            effect.Apply(in context);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & data.TargetLayerMask) == 0) return;

        collision.TryGetComponent<IDamageable>(out var damageable);

        switch (data.HitType)
        {
            case ProjectileHitType.Immediate:
                if (passCount < 0) return;
                else if (passCount > 0)
                {
                    passCount--;
                    if (data.HasAreaPhase)  // 장판 존재
                    {
                        UpdateAreaPhase();
                    }
                    else
                    {
                        HitContext context = GetHitContext(collision);
                        OnValidHit(in context);
                    }
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

    protected override float CalculateDamage()
    {
        return attack.MaxValue * data.DamageMultiplier;
    }

    protected override void UpdateFlyPhase()
    {
        if (!data.HasAreaPhase || passCount > 0) return;
        phaseTimer += Time.deltaTime;
        if (phaseTimer > data.AoEData.FlyPhaseDuration)
        {
            phaseTimer = 0f;
            EnterAreaPhase();
        }
    }

    /// <summary>
    /// 즉발 장판이면 hit, 아니면 장판 소환 시도
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void UpdateAreaPhase()
    {
        if (!data.HasAreaPhase || data.AoEData == null) return;

        Logger.Log("장판 켜짐");

        if (data.AoEData.IsInstant)
        {
            Collider2D[] targets;

            targets = data.AoEData.AoEShape switch
            {
                AoEShape.Circle => Physics2D.OverlapCircleAll(
                    transform.position,
                    data.AoEData.Radius,
                    data.AoEData.AoETargetLayer),
                AoEShape.Box => Physics2D.OverlapBoxAll(
                    transform.position,
                    data.AoEData.BoxSize,
                    360,
                    data.AoEData.AoETargetLayer),
                _ => throw new System.NotImplementedException(),
            };

            foreach (Collider2D target in targets)
            {
                HitContext context = GetHitContext(target);
                data.AoEData.AreaEffects.ForEach(effect => effect.Apply(in context));
            }
        }
        else
        {
            TrySpawnAoE();
        }
    }

    /// <summary>
    /// 장판 소환
    /// </summary>
    private void TrySpawnAoE()
    {
        if (data.AoEData.Prefab == null) return;
        // todo: pool에 넣기
        BaseAoE aoe = Instantiate(data.AoEData.Prefab);
        aoe.Init(data.AoEData, GetHitContext(null));
        aoe.transform.position = transform.position;
    }

    /// <summary>
    /// 수호자 등 가만히 틱 대미지를 주는 애들
    /// </summary>
    protected override void UpdatePersistent()
    {
        tickIntervalTimer += Time.deltaTime;
        if (tickIntervalTimer > data.TickInterval)
        {
            foreach (Collider2D target in targets)
            {
                HitContext context = GetHitContext(target);
                OnValidHit(in context);
            }
            tickIntervalTimer = 0f;
        }
    }

    private HitContext GetHitContext(Collider2D target)
    {
        return new()
        {
            attacker = player,
            damage = CalculateDamage(),
            position = transform.position,
            directTarget = target,
            projectileData = data
        };
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
    }
#endif
}
