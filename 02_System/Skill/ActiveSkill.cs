using System;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    private bool _isReady = true;

    private ActiveSkillData _activeSkillData;

    // 쿨타임
    private float _cooldownTimer = 0f;
    private float _cooldown;

    // 총알
    private ProjectileData _projectileData;
    private ProjectileDataIndex _projectileIndex;

    public override void Init(SkillData data)
    {
        base.Init(data);

        _activeSkillData = data as ActiveSkillData;
        _cooldown = _activeSkillData.Cooldown;
        _projectileData = _activeSkillData.ProjectileData;
        if (!Enum.TryParse(_projectileData.name, true, out _projectileIndex))
        {
            Logger.LogWarning("풀에 사용할 투사체 enum 변환 실패");
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isReady = !_isReady;
        }
        if (_isReady) return;

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
        for (int i = 0; i < _activeSkillData.ProjectilesCounts[CurLevel - 1]; i++)
        {
            PlayerProjectile projectile = ProjectileManager.Instance.Spawn<PlayerProjectile>(_projectileIndex);
            projectile.Init(this, _activeSkillData);
            projectile.Spawn(transform.position, StageManager.Instance.GetNearestMonster());
        }
    }
}
