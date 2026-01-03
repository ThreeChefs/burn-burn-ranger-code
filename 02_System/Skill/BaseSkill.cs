using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 기본 클래스
/// </summary>
public abstract class BaseSkill : MonoBehaviour, IAttackable
{
    #region 필드
    // 스킬 데이터
    protected SkillData skillData;
    protected int skillLevel;

    // 스킬 사용 조건
    protected float cooldownTimer = 0f;
    public bool IsUsable => IsCooldownReady() && HasEnoughResource();

    // 타겟
    protected List<IDamageable> targets;

    // 코루틴
    private Coroutine _coroutine;
    private WaitForSeconds _delay;
    #endregion

    #region 초기화
    public virtual void Init(SkillData data)
    {
        skillData = data;
        _delay = new WaitForSeconds(data.Cooldown);
    }
    #endregion

    protected virtual void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    /// <summary>
    /// 스킬 내부 로직
    /// </summary>
    protected virtual void TryUseSkill()
    {
        if (IsUsable)
        {
            ResetCooldownTimer();

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(nameof(EndSkillCoroutine));
        }
        else
        {
            Logger.Log($"{skillData.Name} 스킬 사용 불가");
        }
    }

    /// <summary>
    /// 스킬 총 시간이 지난 후에 스킬 완료 이벤트 사용하기
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndSkillCoroutine()
    {
        yield return _delay;
    }

    #region 스킬 조건 계산
    /// <summary>
    /// 쿨타임 확인 메서드
    /// </summary>
    /// <returns></returns>
    protected bool IsCooldownReady()
    {
        bool coolReady = cooldownTimer > skillData.Cooldown;
        Logger.Log($"쿨타임 체크: {coolReady}");
        return coolReady;
    }

    /// <summary>
    /// 쿨타임 타이머 초기화
    /// </summary>
    private void ResetCooldownTimer()
    {
        cooldownTimer = 0f;
    }
    #endregion
}
