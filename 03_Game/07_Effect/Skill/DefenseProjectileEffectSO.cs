using UnityEngine;

[CreateAssetMenu(fileName = "New DefenseProjectileEffect", menuName = "SO/Skill/Effect/Defense Projectile")]
public class DefenseProjectileEffectSO : BaseSkillEffectSO
{
    public override void Apply(in HitContext context)
    {
        if (context.directTarget.TryGetComponent<BaseProjectile>(out var projectile))
        {
            projectile.TakeDamage(context.damage);
        }
    }
}
