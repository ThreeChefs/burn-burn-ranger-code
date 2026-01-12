using UnityEngine;

[CreateAssetMenu(fileName = "New KnockbackSkillEffect", menuName = "SO/Skill/Effect/Knockback")]
public class KnockbackEffectSO : BaseEffectSO
{
    public override void Apply(in HitContext context)
    {
        if (context.directTarget == null) return;

        if (context.directTarget.TryGetComponent(out IKnockbackable knockback))
        {
            float force = context.projectileData.KnockBack;
            if (force > 0)
            {
                Vector2 dir = (context.directTarget.transform.position - context.directTarget.transform.position).normalized;
                knockback.ApplyKnockback(dir * force);
                Logger.Log($"{dir * force} 만큼 넉백");
            }
        }
    }
}
