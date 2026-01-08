using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 투사체
/// </summary>
public class PlayerProjectile : BaseProjectile
{
    protected float[] levelValue;

    // 스텟
    protected PlayerStat attackCooldown;
    protected PlayerStat projectileSpeedMultiplier;

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
        PlayerCondition condition = PlayerManager.Instance.Condition;
        attackCooldown = condition[StatType.AttackCooldown];
        projectileSpeedMultiplier = condition[StatType.ProjectileSpeed];
    }

    protected override void Update()
    {
        if (data == null || data.AliveTime < 0) return;
        timer += Time.deltaTime;
        if (timer > data.AliveTime * (1 - attackCooldown.MaxValue))
        {
            gameObject.SetActive(false);
        }
    }

    protected override void FixedUpdate()
    {
        if (data == null) return;
        base.FixedUpdate();
    }
    #endregion

    protected override void ChaseMove()
    {
        Vector3 targetPos = data.Speed * projectileSpeedMultiplier.MaxValue * Time.fixedDeltaTime * targetDir;
        transform.position += targetPos;
    }
}
