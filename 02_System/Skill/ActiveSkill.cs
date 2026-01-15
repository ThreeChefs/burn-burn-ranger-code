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
    protected ProjectileData projectileData;
    protected ProjectileDataIndex projectileIndex;
    protected bool spawnOnce;

    // 코루틴
    private Coroutine _coroutine;
    protected WaitForSeconds projectileSpawnInterval;


    public override void Init(SkillData data)
    {
        base.Init(data);

        activeSkillData = data as ActiveSkillData;

        _cooldownTimer = activeSkillData.Cooldown;
        _cooldown = activeSkillData.Cooldown;

        projectileData = activeSkillData.ProjectileData;
        if (!Enum.TryParse(projectileData.name, true, out projectileIndex))
        {
            Logger.LogWarning("풀에 사용할 투사체 enum 변환 실패");
        }
        spawnOnce = false;

        _attackCooldown = PlayerManager.Instance.Condition[StatType.AttackCooldown];
        projectileSpawnInterval = new WaitForSeconds(activeSkillData.SpawnInterval);
    }

    #region Unity API
    protected override void Update()
    {
        base.Update();
        _cooldownTimer += Time.deltaTime;

        if (!spawnOnce && _cooldownTimer > _cooldown * (1 - _attackCooldown.MaxValue))
        {
            if (activeSkillData.Cooldown < 0)
            {
                spawnOnce = true;
            }

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
    protected virtual IEnumerator UseSkill(Transform target = null)
    {
        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; i++)
        {
            SpawnProjectile();
            yield return projectileSpawnInterval;
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
