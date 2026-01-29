using UnityEngine;

/// <summary>
/// 스킬 - 휘파람 화살
/// </summary>
public class WhistleArrowActiveSkill : ActiveSkill
{
    protected override PlayerProjectile SpawnProjectile()
    {
        Transform target = MonsterManager.Instance.GetNearestMonster();
        return ProjectileManager.Instance.Spawn(projectileIndex, this, target, transform.position);
    }
}
