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

    public override void Init(SkillData data)
    {
        base.Init(data);

        _activeSkillData = data as ActiveSkillData;
        _cooldown = _activeSkillData.Cooldown;
        _projectileData = _activeSkillData.ProjectileData;
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
            // todo: pool에 넣어서 초기화
            GameObject newGo = Instantiate(_projectileData.ProjectilePrefab.gameObject);
            var projectile = newGo.GetComponent<PlayerProjectile>();
            projectile.Init(this, _activeSkillData);
            projectile.Spawn(transform.position);
        }
    }
}
