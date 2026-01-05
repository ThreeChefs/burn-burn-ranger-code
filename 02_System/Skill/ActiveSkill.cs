using UnityEngine;

public class ActiveSkill : BaseSkill
{
    private ActiveSkillData _activeSkillData;

    // 쿨타임
    private float _cooldownTimer = 0f;
    private float _cooldown;

    // 총알
    private GameObject _projectilePrefab;

    public override void Init(SkillData data)
    {
        base.Init(data);

        _activeSkillData = data as ActiveSkillData;
        _cooldown = _activeSkillData.Cooldown;
        _projectilePrefab = _activeSkillData.Projectile;
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
        for (int i = 0; i < _activeSkillData.ProjectilesCounts[CurLevel - 1]; i++)
        {
            GameObject newGo = Instantiate(_projectilePrefab);
            newGo.transform.position = (Vector2)transform.position + _activeSkillData.Offset;
        }
    }
}
