/// <summary>
/// 투사체 이동 로직 조합 클래스
/// </summary>
public static class ProjectileMoveFactory
{
    public static IProjectileMove CreateMove(BaseProjectile projectile, ProjectileData data)
    {
        IProjectileMove move;
        move = CreateBaseMove(projectile, data);

        if ((data.MoveFeature & ProjectileMoveFeature.Reflection) != 0)
        {
            move = new ReflectionMove(projectile, move, data.ReflectionLayerMask);
        }

        if ((data.MoveFeature & ProjectileMoveFeature.Guidance) != 0)
        {
            move = new GuidanceMove(projectile, move, data.GuidanceTime);
        }

        return move;
    }

    private static IProjectileMove CreateBaseMove(BaseProjectile projectile, ProjectileData data)
    {
        return data.BaseMoveType switch
        {
            ProjectileBaseMoveType.Straight => new StraightMove(projectile),
            _ => null
        };
    }
}
