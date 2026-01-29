using System;

public enum ProjectilePhase
{
    Fly,
    Area
}

public enum ProjectileBaseMoveType
{
    Stop,
    Straight,
    // 추후 투사체 기본 이동 추가 (ex. 회전)
}

[Flags]
public enum ProjectileMoveFeature
{
    None = 0,
    Guidance = 1 << 0,
    Reflection = 1 << 1,
}

public enum ProjectileHitType
{
    Immediate,      // 충돌 시점
    Persistent,     // 유지
    Timed,          // 특정 시점
}

public enum AoEShape
{
    Circle,     // 원형 폭발
    Box         // 사각형 폭발
}

public enum ProjectileSfxType
{
    Hit,
    Explode,
    SpawnOnce,
    SpawnLoop,
    AoE,

    None = 100,
}

public enum ProjectileDataIndex     // SO 이름과 동일하게 맞추기
{
    // 스킬
    KunaiProjectileData,            // 쿠나이
    GhostShurikenProjectileData,    // 유령수리검
    FireBombProjectileData,         // 화염병
    RocketProjectileData,           // 로켓
    ShieldProjectileData,           // 방어막
    GravityFieldProjectileData,     // 중력장
    SoccerBallProjectileData,       // 축구공
    BrickProjectileData,            // 벽돌
    DumbelProjectileData,           // 덤벨
    DronProjectileData,             // 드론
    QuantumBallProjectileData,      // 양자공
    ThunderProjectileData,          // 번개
    ThunderBatteryProjectileData,   // 천둥배터리
    DurianProjectileData,           // 두리안
    DrillShotProjectileData,        // 드릴
    SharkBeakCannonProjectileData,  // 상어포
    BoomerangProjectileData,        // 부메랑
    ThornSpearProjectileData,       // 가시창 원본
    ThornSpearChildProjectileData,  // 가시창 가시
    FlyingBallProjectileData,       // 플라잉볼 (부메랑->마그네틱다트)
    WhistleArrowProjectileData,     // 휘파람 화살
    LaserProjectileData,            // 레이저
    DeathLaserProjectileData,       // 죽음의 레이저
    GuardianProjectileData,         // 수호자
    SuperGuardianProjectileData,    // 수비수
    PoisonArea,
    RangedAttack,                   // 
    DragonProjectile,
    // 공통
    RangedProjectile


}
