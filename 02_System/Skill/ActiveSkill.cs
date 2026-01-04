using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // 쿨타임
    private float _cooldownTimer = 0f;
    private float Cooldown => ((ActiveSkillData)skillData).Cooldown;

    // 타겟
    private List<IDamageable> _targets;

    public override void Init(SkillData data)
    {
        base.Init(data);

        _targets = new();
    }

    protected override void Update()
    {
        base.Update();
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer > Cooldown)
        {
            _cooldownTimer = 0f;
            UseSkill();
        }
    }

    /// <summary>
    /// 스킬 내부 로직
    /// </summary>
    private void UseSkill()
    {
        _targets.ForEach(t => t.TakeDamage(100f));
    }
}
