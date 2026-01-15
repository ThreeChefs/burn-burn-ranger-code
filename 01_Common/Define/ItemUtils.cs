using UnityEngine;

public static class ItemUtils
{
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
