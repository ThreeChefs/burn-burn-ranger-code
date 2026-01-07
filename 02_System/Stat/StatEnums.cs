public enum StatType
{
    Health,
    Heal,                       // 5초마다 1% 씩회복 할때 사용
    Stamina,
    Attack,
    Defense,
    SkillRange,                 // 습득시 스킬무기의 범위증가 용도
    AttackCooldown,
    Speed,
    AddEXP,                     // 경험치 획득량 6%씩 증가
    AddGold,                    // 골드 획득량 8% 씩 증가
    DropItemRange,              // 드랍된 아이템 습득 범위 증가 (자석)
    DamageReduction,            // 대미지 감소
    ProjectileAliveDuration,    // 효과 지속 = 공격 지속 시간
    ProjectileSpeed,            // 탄환 속도
}