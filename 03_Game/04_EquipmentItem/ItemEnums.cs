// 아이템 타입
public enum ItemType
{
    General,
    Equipment,
    UpgradeMaterial,    // 업그레이드 재화 / 슬롯 표시를 위한 타입분류
}

// 아이템 등급
public enum ItemClass
{
    None,
    Normal,
    Rare,
    Elite,
    Epic,
    Legendary,
}

// 장비 아이템 타입
public enum EquipmentType
{
    Weapon,
    Armor,
    Necklace,
    Gloves,
    Belt,
    Shoes,
}

// 장비 효과
public enum EquipmentEffectType
{
    Stat,           // 스탯 변경
    Buff,           // 버프 부여
    Skill,          // 스킬 부여
}

// 적용 방식
public enum EffectApplyType
{
    Flat,       // 고정값
    Percent,    // 퍼센트
}

// 조건
public enum EffectTriggerType
{
    Always,
    OnCondition,
    OnKill,
    OnHit,
}

// 대상
public enum EffectTargetType
{
    Self,
    AllEnemy,
    NormalEnemy,
    BossEnemy,
}

// 버프 타입
public enum BuffType
{
    Invincible
}

// 장비 적용 타입
public enum EquipmentApplyType
{
    Equip,
    Unequip
}