using System;

public enum SkillType
{
    Active,
    Passive,
    Combination,
}

public enum SkillValueType
{
    AttackPower,        // (액티브) 공격력
    StatBuff,           // (패시브) 스텟 버프
    ProjectileCount,    // (액티브) 탄환 개수
    Scale,              // (액티브) 크기
    ProjectileSpeed,    // (액티브) 탄환 속도
}

[Flags]
public enum SkillState : byte
{
    None = 0,
    CanDraw = 1 << 0,
    CombinationReady = 1 << 1,
    LockedByMax = 1 << 2,
    LockedByCount = 1 << 3,
}