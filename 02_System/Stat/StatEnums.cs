using System.Collections.Generic;

public enum StatType
{
    Health,
    Heal,                       // 5초마다 1% 씩회복 할때 사용
    Stamina,
    Attack,
    Defense,
    ProjecttileRange,                 // 습득시 스킬무기의 범위증가 용도
    AttackCooldown,
    Speed,
    AddEXP,                     // 경험치 획득량 6%씩 증가
    AddGold,                    // 골드 획득량 8% 씩 증가
    DropItemRange,              // 드랍된 아이템 습득 범위 증가 (자석)
    DamageReduction,            // 대미지 감소
    ProjectileAliveDuration,    // 효과 지속 = 공격 지속 시간
    ProjectileSpeed,            // 탄환 속도
}

public static class StatTypeText
{
    /// <summary>
    /// 스탯 설명
    /// ex)공격력
    /// </summary>
    public static readonly Dictionary<StatType, string> StatDescriptionName = new()
    {
        { StatType.Health, "HP" },
        { StatType.Heal, "고기 회복" },
        { StatType.Attack, "공격력" },
        { StatType.Defense, "방어력" },
    };

    /// <summary>
    /// 스탯 이름
    /// </summary>
    public static readonly Dictionary<StatType, string> StatName = new()
    {
        { StatType.Health, "체력" },
        { StatType.Heal, "회복" },
        { StatType.Attack, "힘" },
        { StatType.Defense, "끈기" },
    };

    /// <summary>
    /// 스탯 설명
    /// </summary>
    public static readonly Dictionary<StatType, string> StatGrowthDesc = new()
    {
        { StatType.Health, "체력이 세면 파워도 셉니다." },
        { StatType.Heal, "음식들은 삶에 활력을 줍니다." },
        { StatType.Attack, "적의 얼굴을 강타합니다." },
        { StatType.Defense, "고통 없이는 얻는 것도 없다." },
    };

}