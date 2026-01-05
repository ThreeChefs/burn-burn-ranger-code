using System;
using UnityEngine;

/// <summary>
/// 스탯 클래스
/// </summary>
public class PlayerStat : BaseStat
{
    // 필드
    public float EquipmentValue { get; private set; }
    public float BuffValue { get; private set; }

    public override float MaxValue => BaseValue + EquipmentValue + BuffValue;

    // 이벤트
    public event Action<float> OnMaxValueChanged;

    public PlayerStat(float value, StatType type) : base(value, type) { }

    /// <summary>
    /// [public] Stat 보유 클래스가 사라질 경우 메모리 정리를 위해 호출
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        OnMaxValueChanged = null;
    }

    /// <summary>
    /// [public] 장비 아이템 착용으로 변경된 스텟 값 적용
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEquipmentValue(float value)
    {
        EquipmentValue = value;

        CurValue = Mathf.Min(CurValue, MaxValue);

        OnCurValueChanged?.Invoke(CurValue);
        OnMaxValueChanged?.Invoke(MaxValue);
    }

    public void UpdateBuffValue(float value)
    {
        BuffValue = value;
        CurValue = Mathf.Min(CurValue + BuffValue, MaxValue);
    }
}
