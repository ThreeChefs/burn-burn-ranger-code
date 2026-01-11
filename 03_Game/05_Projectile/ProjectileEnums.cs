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

public enum ExplosionShape
{
    Circle,     // 원형 폭발
    Box         // 사각형 폭발
}

public enum ProjectileDataIndex     // SO 이름과 동일하게 맞추기
{
    // 스킬
    KunaiProjectileData,            // 쿠나이
    GhostShurikenProjectileData,
    RangedAttack,
    FireBombProjectileData,
    RocketProjectileData,

    // 공통
    RangedProjectile
}
