using UnityEngine;

[CreateAssetMenu(fileName = "New DamageSkillEffect", menuName = "SO/Skill/Effect/Damage")]
public class DamageSkillEffectSO : BaseSkillEffectSO
{
    public override void Apply(in HitContext context)
    {
        if (context.directTarget == null) return;

        if (context.directTarget.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(context.damage);
            Logger.Log($"대미지: {context.damage}");
        }
    }
}
