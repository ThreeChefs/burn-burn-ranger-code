using System;
using UnityEngine;

public class BaseStat
{
    // 필드
    protected StatType type;

    public float BaseValue { get; protected set; }
    public float CurValue { get; protected set; }

    public virtual float MaxValue => BaseValue;

    // 이벤트
    public Action<float> OnCurValueChanged;

    public BaseStat(float value, StatType type)
    {
        BaseValue = value;
        CurValue = value;
        this.type = type;
    }

    /// <summary>
    /// [public] Stat 보유 클래스가 사라질 경우 메모리 정리를 위해 호출
    /// </summary>
    public virtual void OnDestroy()
    {
        OnCurValueChanged = null;
    }

    /// <summary>
    /// [public] 특정 값 만큼 스텟 값 사용 시도. 변경 다음 OnValueChanged 호출
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public virtual bool TryUse(float amount)
    {
        if (CurValue < amount)
        {
            Logger.Log($"{type} 부족");
            return false;
        }

        CurValue -= amount;
        OnCurValueChanged?.Invoke(CurValue);
        return true;
    }

    /// <summary>
    /// [public] 특정 값 만큼 획득
    /// </summary>
    /// <param name="amount"></param>
    public virtual void Add(float amount)
    {
        CurValue = Mathf.Min(CurValue + amount, MaxValue);
        OnCurValueChanged?.Invoke(CurValue);
    }

    public void ResetCurValue()
    {
        CurValue = MaxValue;
        Logger.Log($"값 초기화: {CurValue} / {MaxValue}");
    }
}
