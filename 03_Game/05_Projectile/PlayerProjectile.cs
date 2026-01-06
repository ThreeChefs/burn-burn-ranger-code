using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 투사체
/// </summary>
public class PlayerProjectile : BaseProjectile
{
    protected ActiveSkill skill;
    protected float[] levelValue;

    public void Init(ActiveSkill skill, ActiveSkillData data)
    {
        this.skill = skill;
        levelValue = data.LevelValue;

        base.Init(PlayerManager.Instance.Condition[StatType.Attack], data.ProjectileData);
    }

    public override void Spawn(Transform target)
    {
        targetPos = StageManager.Instance.GetNearestMonster().position;
        transform.position = target.position;

        targetDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
