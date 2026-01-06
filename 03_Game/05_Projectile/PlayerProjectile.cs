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
        Vector2 pos = target.position;
        targetPos = StageManager.Instance.GetNearestMonster().position;
        transform.position = pos + (Vector2)(targetPos - transform.position).normalized;
    }
}
