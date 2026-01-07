using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 투사체
/// </summary>
public class PlayerProjectile : BaseProjectile
{
    protected ActiveSkill skill;
    protected float[] levelValue;

    // 스텟
    protected PlayerStat projectileSpeedMultiplier;
    protected PlayerStat projectileAliveDuration;

    protected override void Update()
    {
        if (data.AliveTime < 0) return;
        timer += Time.deltaTime;
        if (timer > data.AliveTime * (1 - projectileAliveDuration.MaxValue))
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(ActiveSkill skill, ActiveSkillData data)
    {
        this.skill = skill;
        levelValue = data.LevelValue;
        PlayerCondition condition = PlayerManager.Instance.Condition;

        base.Init(condition[StatType.Attack], data.ProjectileData);

        // 스텟 캐싱
        projectileSpeedMultiplier = condition[StatType.ProjectileSpeed];
        projectileAliveDuration = condition[StatType.ProjectileAliveDuration];
    }

    protected override void ChaseMove()
    {
        Vector3 targetPos = data.Speed * projectileSpeedMultiplier.MaxValue * Time.fixedDeltaTime * targetDir;
        transform.position += targetPos;
    }
}
