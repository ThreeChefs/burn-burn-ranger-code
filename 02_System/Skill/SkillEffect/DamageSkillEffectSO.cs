public class DamageSkillEffectSO : BaseSkillEffectSO
{
    public override void Apply(in HitContext context)
    {
        if (context.directTarget == null) return;

        if (context.directTarget.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(context.damage);
        }
    }
}
