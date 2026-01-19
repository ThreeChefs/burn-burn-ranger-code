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
                knockback.ApplyKnockback(context.attacker.position, force);
                Logger.Log($"{force} 만큼 넉백");
            }
        }
    }
}
