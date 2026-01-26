using UnityEngine;

[CreateAssetMenu(fileName = "New DamageSkillEffect", menuName = "SO/Skill/Effect/Damage")]
public class DamageEffectSO : BaseSkillEffectSO
{
    public override void Apply(in HitContext context)
    {
        if (context.directTarget == null) return;

        if (context.directTarget.TryGetComponent(out Monster monster))
        {
            monster.TakeDamage(context.damage);
        }
    }
}
