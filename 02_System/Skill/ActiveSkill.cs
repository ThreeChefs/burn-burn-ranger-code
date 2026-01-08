using System;
using System.Collections;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    // 캐싱
    private ActiveSkillData _activeSkillData;
    private BaseStat _attackCooldown;

    // 쿨타임
    private float _cooldownTimer = 0f;
    private float _cooldown;

    // 총알
    private ProjectileData _projectileData;
    private ProjectileDataIndex _projectileIndex;

    // 코루틴
    private Coroutine _coroutine;
    private WaitForSeconds _projectileDelay;

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

        _attackCooldown = PlayerManager.Instance.Condition[StatType.AttackCooldown];
        _projectileDelay = new WaitForSeconds(0.1f);
    }

    #region Unity API
    protected override void Update()
    {
        base.Update();
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer > _cooldown * (1 - _attackCooldown.MaxValue))
        {
            Transform target = MonsterManager.Instance.GetNearestMonster();
            if (target == null) return;

            StopPlayingCoroutine();
            _coroutine = StartCoroutine(UseSkill(target));

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
    private IEnumerator UseSkill(Transform target)
    {
        for (int i = 0; i < _activeSkillData.ProjectilesCounts[CurLevel - 1]; i++)
        {
            ProjectileManager.Instance.Spawn(
                _projectileIndex,
                PlayerManager.Instance.Condition[StatType.Attack],
                target,
                _activeSkillData,
                transform.position);
            yield return _projectileDelay;
        }
    }

    private void StopPlayingCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
