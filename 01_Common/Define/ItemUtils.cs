using UnityEngine;

/// <summary>
/// 아이템 클래스 색상
/// </summary>
public static class ItemClassColor
{
    // 아이템 클래스 색상
    public static readonly Color ColorNormal = new(0.75f, 0.75f, 0.75f); // 회색
    public static readonly Color ColorRare = new(0.30f, 0.69f, 0.31f); // 녹색
    public static readonly Color ColorElite = new(0.13f, 0.59f, 0.95f); // 파랑
    public static readonly Color ColorEpic = new(0.74f, 0.35f, 0.90f); // 연보라
    public static readonly Color ColorLegendary = new(1.00f, 0.60f, 0.00f); // 오렌지

    public static Color GetGradeColor(ItemClass itemClass = ItemClass.None)
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
        Color baseColor = GetGradeColor(itemClass);
        return baseColor * 1.15f;
    }
}


public static class ItemUtils
{
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
