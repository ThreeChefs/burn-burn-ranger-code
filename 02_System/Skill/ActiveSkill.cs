using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // 쿨타임
    private float _cooldownTimer = 0f;
    private float _cooldown;

    // 총알
    private GameObject _projectilePrefab;

    // 타겟
    private List<IDamageable> _targets;

    public override void Init(SkillData data)
    {
        base.Init(data);

        var activeSkillData = data as ActiveSkillData;
        _cooldown = activeSkillData.Cooldown;
        _projectilePrefab = activeSkillData.Projectile;

        _targets = new();
    }

    protected override void Update()
    {
        base.Update();
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer > _cooldown)
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
        Instantiate(_projectilePrefab);
    }
}
