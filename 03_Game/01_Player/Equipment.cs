using System;
using System.Collections.Generic;

/// <summary>
/// 장비 장착 관리
/// </summary>
public class Equipment
{
    // 캐싱
    private readonly PlayerCondition _condition;

    private readonly Dictionary<EquipmentType, ItemInstance> _equipments;
    public IReadOnlyDictionary<EquipmentType, ItemInstance> Equipments => _equipments;

    // 이벤트
    public event Action OnEquipmentChanged;

    public Equipment(PlayerCondition condition)
    {
        // 캐싱
        _condition = condition;

        _equipments = new();

        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            _equipments[type] = null;
        }

        // todo: 장비 데이터 저장 / 로드
        foreach (ItemInstance item in _equipments.Values)
        {
            ApplyEquipmentValue(item, EquipmentApplyType.Equip);
        }
    }

    public void OnDestroy()
    {
        OnEquipmentChanged = null;
    }

    #region [public] 장비 장착 / 해제
    /// <summary>
    /// [public] 장비 장착
    /// </summary>
    /// <param name="item"></param>
    public void Equip(ItemInstance item)
    {
        EquipmentType type = item.ItemData.EquipmentType;
        Unequip(item);

        _equipments[type] = item;
        ApplyEquipmentValue(item, EquipmentApplyType.Equip);
    }

    /// <summary>
    /// [public] 장비 장착 해제
    /// </summary>
    /// <param name="item"></param>
    public void Unequip(ItemInstance item)
    {
        EquipmentType type = item.ItemData.EquipmentType;
        if (_equipments.TryGetValue(type, out ItemInstance prev))
        {
            _equipments[type] = null;
            if (prev != null)
            {
                ApplyEquipmentValue(prev, EquipmentApplyType.Unequip);
            }
        }
        OnEquipmentChanged?.Invoke();
    }

    /// <summary>
    /// [public] 현재 장착하고 있는 아이템인지 확인
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsEquip(ItemInstance item)
    {
        if (!_equipments.TryGetValue(item.ItemData.EquipmentType, out ItemInstance equipment))
        {
            return false;
        }

        return equipment != null && equipment.Equals(item);
    }
    #endregion

    #region 수치 계산
    /// <summary>
    /// 단일 장비 수치 계산
    /// </summary>
    /// <param name="item"></param>
    /// <param name="type"></param>
    private void ApplyEquipmentValue(ItemInstance item, EquipmentApplyType type)
    {
        int sign = type == EquipmentApplyType.Equip ? 1 : -1;

        if (item == null) return;

        // 장비 자체 수치 계산
        (StatType statType, int value) = item.GetStatAndValue();
        _condition[statType].UpdateEquipmentValue(value * sign);

        // 장비 등급에 따른 수치 계산
        foreach (var equipmentEffect in item.ItemData.Equipments)
        {
            switch (equipmentEffect.EffectType)
            {
                case EquipmentEffectType.Stat:
                    UpdateStat(equipmentEffect.ApplyType, equipmentEffect.Stat, equipmentEffect.Value * sign);
                    break;
            }
        }
    }

    /// <summary>
    /// 스텟 값 업데이트
    /// </summary>
    /// <param name="applyType"></param>
    /// <param name="statType"></param>
    /// <param name="value"></param>
    private void UpdateStat(EffectApplyType applyType, StatType statType, int value)
    {
        switch (applyType)
        {
            case EffectApplyType.Flat:
                _condition[statType].UpdateEquipmentValue(value);
                break;
            case EffectApplyType.Percent:
                _condition[statType].UpdateEquipmentValue(_condition[statType].BaseValue * value * 0.01f);
                break;
        }
    }
    #endregion
}
