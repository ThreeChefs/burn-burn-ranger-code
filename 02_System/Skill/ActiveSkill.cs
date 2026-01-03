using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // 스킬 사용 조건
    private float _cooldownTimer = 0f;

    // 타겟
    private List<IDamageable> _targets;

    public override void Init(SkillData data)
    {
        base.Init(data);
    }

    protected override void Update()
    {
        base.Update();
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer > ((ActiveSkillData)skillData).Cooldown)
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
