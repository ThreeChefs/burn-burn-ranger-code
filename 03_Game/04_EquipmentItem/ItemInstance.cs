using System;
using UnityEngine;

/// <summary>
/// 보유하고 있는 실제 아이템 데이터
/// </summary>
[Serializable]
public class ItemInstance
{
    [field: SerializeField] public ItemClass ItemClass { get; private set; }
    [field: SerializeField] public ItemData ItemData { get; private set; }
    [field: SerializeField] public int Count { get; private set; }
    [field: SerializeField] public int Level { get; private set; }

    public event Action OnLevelChanged;

    private ItemDatabase ItemDatabase => GameManager.Instance.ItemDatabase;

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

    public ItemInstance(ItemClass itemClass, int id, int count, int level)
    {
        ItemClass = itemClass;
        ItemData = ItemDatabase.FindById(id);
        Count = count;
        Level = level;
    }

    /// <summary>
    /// [public] 레벨업하기
    /// </summary>
    /// <returns></returns>
    public bool TryLevelUp()
    {
        if (CheckWallet())
        {
            Level++;
            OnLevelChanged?.Invoke();
            CheckEquip();

            return true;
        }

        Logger.Log("재화 부족");
        return false;
    }

    /// <summary>
    /// [public] 일괄 레벨업하기
    /// </summary>
    /// <returns></returns>
    public bool TryAllLevelUp()
    {
        bool doneLevelUp = false;

        while (CheckWallet())
        {
            doneLevelUp = true;
            Level++;
        }

        if (doneLevelUp)
        {
            OnLevelChanged?.Invoke();
            CheckEquip();
        }

        return doneLevelUp;
    }

    /// <summary>
    /// 재화 정보 사용할 수 있는지 확인하기
    /// </summary>
    /// <returns></returns>
    private bool CheckWallet()
    {
        return PlayerManager.Instance.Wallet[WalletType.Gold].TryUse(GetUpgradeGold())
            && PlayerManager.Instance.Wallet[ItemUtils.GetRequiringScrollType(ItemData.EquipmentType)].TryUse(GetUpgradeScroll());
    }

    private void CheckEquip()
    {
        Equipment equipment = PlayerManager.Instance.Equipment;
        if (equipment.IsEquip(this))
        {
            equipment.Equip(this);
        }
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
