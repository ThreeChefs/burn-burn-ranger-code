public enum ProjectilePhase
{
    Fly,
    Area
}

public enum ProjectileMoveType
{
    Straight,       // 직선
    Guidance,       // 유도
    Reflection,     // 반사
}

public enum ProjectileHitType
{
    Immediate,      // 충돌 시점
    Persistent,     // 유지
    Timed,          // 특정 시점
}

public enum ProjectileAnchorType
{
    World,      // 월드 좌표 고정
    Owner       // 시전자(플레이어)를 따라감
}

public enum AoEShape
{
    Circle,     // 원형 폭발
    Box         // 사각형 폭발
}

public enum ProjectileDataIndex     // SO 이름과 동일하게 맞추기
{
    // 스킬
    KunaiProjectileData,            // 쿠나이
    GhostShurikenProjectileData,    // 유령수리검
    FireBombProjectileData,         // 화염병
    RocketProjectileData,           // 로켓
    ShieldProjectileData,           // 방어막
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

    RangedAttack,                   // 

    // 공통
    RangedProjectile
}
