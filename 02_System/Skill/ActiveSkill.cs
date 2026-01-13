using System;
using System.Collections;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // 캐싱
    protected ActiveSkillData activeSkillData;
    public ActiveSkillData Data => activeSkillData;
    private BaseStat _attackCooldown;

    // 쿨타임
    private float _cooldownTimer;
    private float _cooldown;

    // 총알
    private ProjectileData _projectileData;
    protected ProjectileDataIndex projectileIndex;

    // 코루틴
    private Coroutine _coroutine;
    private WaitForSeconds _projectileSpawnInterval;

    public override void Init(SkillData data)
    {
        base.Init(data);

        activeSkillData = data as ActiveSkillData;

        _cooldownTimer = activeSkillData.Cooldown;
        _cooldown = activeSkillData.Cooldown;

        _projectileData = activeSkillData.ProjectileData;

        if (!Enum.TryParse(_projectileData.name, true, out projectileIndex))
        {
            Logger.LogWarning("풀에 사용할 투사체 enum 변환 실패");
        }

        _attackCooldown = PlayerManager.Instance.Condition[StatType.AttackCooldown];
        _projectileSpawnInterval = new WaitForSeconds(activeSkillData.SpawnInterval);
    }

    #region Unity API
    protected override void Update()
    {
        base.Update();
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer > _cooldown * (1 - _attackCooldown.MaxValue))
        {
            StopPlayingCoroutine();
            _coroutine = StartCoroutine(UseSkill());

            _cooldownTimer = 0f;
        }
    }

    protected override void OnDestroy()
    {
        StopPlayingCoroutine();
        base.OnDestroy();
    }
    #endregion

    /// <summary>
    /// 스킬 내부 로직
    /// </summary>
    protected virtual IEnumerator UseSkill(Transform transform = null)
    {
        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; i++)
        {
            SpawnProjectile();
            yield return _projectileSpawnInterval;
        }
    }

    /// <summary>
    /// 투사체 소환
    /// </summary>
    /// <returns></returns>
    protected virtual PlayerProjectile SpawnProjectile()
    {
        Transform target = MonsterManager.Instance.GetNearestMonster();
        if (target == null) return null;

        return ProjectileManager.Instance.Spawn(projectileIndex, this, target, transform.position);
    }

    protected void StopPlayingCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
