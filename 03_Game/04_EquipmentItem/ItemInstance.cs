using System;
using UnityEngine;

/// <summary>
/// 보유하고 있는 실제 아이템 데이터
/// </summary>
public class ItemInstance
{
    public ItemClass ItemClass { get; private set; }
    public ItemData ItemData { get; private set; }
    public int Count { get; private set; }
    public int Level { get; private set; }

    public event Action OnLevelChanged;

    /// <summary>
    /// [public] 생성자
    /// </summary>
    /// <param name="itemClass"></param>
    /// <param name="itemData"></param>
    public ItemInstance(ItemClass itemClass, ItemData itemData)
    {
        ItemClass = itemClass;
        ItemData = itemData;
        Count = 1;
        Level = 1;
    }

    /// <summary>
    /// [public] 레벨업하기
    /// </summary>
    /// <returns></returns>
    public bool TryLevelUp()
    {
        // todo: 스크롤 추가
        if (!PlayerManager.Instance.Wallet[WalletType.Gold].TryUse(GetUpgradeGold()))
        {
            Logger.Log("재화 부족");
            return false;
        }

        Level++;
        OnLevelChanged?.Invoke();

        PlayerManager.Instance.Equipment.Equip(this);

        return true;
    }

    public bool TryAllLevelUp()
    {
        bool doneLevelUp = false;

        // todo: 스크롤 추가
        while (PlayerManager.Instance.Wallet[WalletType.Gold].TryUse(GetUpgradeGold()))
        {
            doneLevelUp = true;
            Level++;
        }

        if (doneLevelUp)
        {
            OnLevelChanged?.Invoke();
            PlayerManager.Instance.Equipment.Equip(this);
        }

        return doneLevelUp;
    }

    #region Utils - 수치 계산
    public (StatType, int) GetStatAndValue()
    {
        EquipmentType type = ItemData.EquipmentType;
        int defaultValue = ItemUtils.GetDefaultStatValue(ItemClass, type);
        int levelValue = CalcLevelValue();

        return (ItemUtils.GetStatType(type), defaultValue + levelValue);
    }

    private int CalcLevelValue()
    {
        // todo: 장비에 따라 수치 다르게 계산하도록 수정
        return Level * 20;
    }

    public int GetUpgradeGold()
    {
        const int a = 25;
        const int b = 50;
        return Mathf.RoundToInt(a * Level * Level + b * Level);
    }

    public int GetUpgradeScroll()
    {
        return Level;
    }
    #endregion

    public override string ToString()
    {
        return $"ItemClass: {ItemClass} " + ItemData.ToString();
    }
}
