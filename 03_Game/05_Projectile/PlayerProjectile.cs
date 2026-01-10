using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 투사체
/// </summary>
public class PlayerProjectile : BaseProjectile
{
    // 캐싱
    protected Transform player;
    protected SkillConfig skillConfig;

    protected float[] levelValue;

    // 스텟
    protected PlayerStat attackCooldown;
    protected PlayerStat projectileSpeed;

    protected override float Speed => base.Speed * projectileSpeed.MaxValue;

    public override void Init(BaseStat attack, ScriptableObject originData)
    {
        ActiveSkillData data = originData as ActiveSkillData;
        skillConfig = data.SkillConfig;
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

    private void OnValidHit(in HitContext context)
    {
        foreach (BaseSkillEffectSO effect in skillConfig.Effects)
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
                HitContext context = GetHitContext(collision);
                OnValidHit(in context);
                if (passCount < 0) return;
                passCount--;
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

    protected override void UpdateAreaPhase()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer < data.TickInterval) return;

        tickTimer = 0f;

        foreach (Collider2D target in targets)
        {
            HitContext context = GetHitContext(target);
            OnValidHit(in context);
        }
    }

    private HitContext GetHitContext(Collider2D target)
    {
        return new()
        {
            attacker = player,
            damage = CalculateDamage(),
            position = targetPos,
            directTarget = target,
            projectileData = data
        };
    }
}
