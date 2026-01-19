using UnityEngine;

public static class ItemUtils
{
    #region 아이템 등급 - 색상
    public static readonly Color ColorNormal = new(0.75f, 0.75f, 0.75f); // 회색
    public static readonly Color ColorRare = new(0.30f, 0.69f, 0.31f); // 녹색
    public static readonly Color ColorElite = new(0.13f, 0.59f, 0.95f); // 파랑
    public static readonly Color ColorEpic = new(0.74f, 0.35f, 0.90f); // 연보라
    public static readonly Color ColorLegendary = new(1.00f, 0.60f, 0.00f); // 오렌지

    public static Color GetClassColor(ItemClass itemClass = ItemClass.None)
    {
        return itemClass switch
        {
            ItemClass.Normal => ColorNormal,
            ItemClass.Rare => ColorRare,
            ItemClass.Elite => ColorElite,
            ItemClass.Epic => ColorEpic,
            ItemClass.Legendary => ColorLegendary,
            _ => Color.white
        };
    }

    /// <summary>
    /// 아이콘 외곽선 / 강조용 컬러
    /// </summary>
    public static Color GetHighlightColor(ItemClass itemClass)
    {
        Color baseColor = GetClassColor(itemClass);
        return baseColor * 1.15f;
    }
    #endregion

    #region 아이템 등급 - 문자열
    /// <summary>
    /// 아이템 등급 문자열 반환
    /// </summary>
    /// <param name="itemClass"></param>
    /// <returns></returns>
    public static string GetClassString(ItemClass itemClass = ItemClass.None)
    {
        return itemClass switch
        {
            ItemClass.Normal => "일반",
            ItemClass.Rare => "레어",
            ItemClass.Elite => "엘리트",
            ItemClass.Epic => "에픽",
            ItemClass.Legendary => "레전더리",
            _ => "None"
        };
    }
    #endregion

    #region 아이템 등급 - 레벨
    public static int GetClassMaxLevel(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 5,
            ItemClass.Rare => 10,
            ItemClass.Elite => 15,
            ItemClass.Epic => 20,
            ItemClass.Legendary => 25,
            _ => 1
        };
    }
    #endregion

    #region 아이템 등급 - 스텟 수치
    public static int GetDefaultStatValue(ItemClass itemClass, EquipmentType equipmentType)
    {
        return equipmentType switch
        {
            EquipmentType.Weapon => GetWeaponValue(itemClass),
            EquipmentType.Necklace => GetNecklaceValue(itemClass),
            EquipmentType.Gloves => GetGlovesValue(itemClass),
            EquipmentType.Armor => GetArmorValue(itemClass),
            EquipmentType.Belt => GetBeltValue(itemClass),
            EquipmentType.Shoes => GetShoesValue(itemClass),
            _ => 0,
        };
    }

    private static int GetWeaponValue(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 10,
            ItemClass.Rare => 20,
            ItemClass.Elite => 30,
            ItemClass.Epic => 50,
            ItemClass.Legendary => 100,
            _ => 10
        };
    }

    private static int GetArmorValue(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 50,
            ItemClass.Rare => 70,
            ItemClass.Elite => 100,
            ItemClass.Epic => 200,
            ItemClass.Legendary => 500,
            _ => 10
        };
    }

    private static int GetNecklaceValue(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 7,
            ItemClass.Rare => 14,
            ItemClass.Elite => 21,
            ItemClass.Epic => 35,
            ItemClass.Legendary => 70,
            _ => 10
        };
    }

    private static int GetGlovesValue(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 8,
            ItemClass.Rare => 16,
            ItemClass.Elite => 24,
            ItemClass.Epic => 40,
            ItemClass.Legendary => 80,
            _ => 10
        };
    }

    private static int GetBeltValue(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 38,
            ItemClass.Rare => 53,
            ItemClass.Elite => 75,
            ItemClass.Epic => 150,
            ItemClass.Legendary => 300,
            _ => 10
        };
    }

    private static int GetShoesValue(ItemClass itemClass)
    {
        return itemClass switch
        {
            ItemClass.Normal => 38,
            ItemClass.Rare => 53,
            ItemClass.Elite => 75,
            ItemClass.Epic => 150,
            ItemClass.Legendary => 300,
            _ => 10
        };
    }
    #endregion

    #region 아이템 타입
    public static StatType GetStatType(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
            case EquipmentType.Necklace:
            case EquipmentType.Gloves:
                return StatType.Attack;
            case EquipmentType.Armor:
            case EquipmentType.Belt:
            case EquipmentType.Shoes:
                return StatType.Health;
            default:
                throw new System.NotImplementedException();
        }
    }

    public static WalletType GetRequiringScrollType(EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Weapon => WalletType.UpgradeMaterial_Weapon,
            EquipmentType.Necklace => WalletType.UpgradeMaterial_Necklace,
            EquipmentType.Gloves => WalletType.UpgradeMaterial_Gloves,
            EquipmentType.Armor => WalletType.UpgradeMaterial_Armor,
            EquipmentType.Belt => WalletType.UpgradeMaterial_Belt,
            EquipmentType.Shoes => WalletType.UpgradeMaterial_Shoes,
        };
    }
    #endregion

#if UNITY_EDITOR
    // 아이템 집어넣을 때 id 값 편하게 하려고 만든 값
    public static int ItemNumber = 0;
    public static int GetItemNumber()
    {
        return ItemNumber++;
    }

    public static void SetItemNumber(int num)
    {
        ItemNumber = num;
    }
#endif
}
