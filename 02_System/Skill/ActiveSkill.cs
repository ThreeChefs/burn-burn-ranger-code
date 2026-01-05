using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    private ActiveSkillData _activeskillData;

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

        _activeskillData = data as ActiveSkillData;
        _cooldown = _activeskillData.Cooldown;
        _projectilePrefab = _activeskillData.Projectile;

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
        for (int i = 0; i < _activeskillData.ProjectilesCounts[CurLevel - 1]; i++)
        {
            Instantiate(_projectilePrefab);
            // todo: 위치는 플레이어한테 지정
        }
    }
}
