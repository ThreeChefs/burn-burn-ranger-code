using UnityEngine;

public class ProjectileManager : PoolManager<ProjectileManager, ProjectileDataIndex>
{
    /// <summary>
    /// 공격력과 타겟을 받는 스폰
    /// </summary>
    public BaseProjectile Spawn(ProjectileDataIndex poolIndex, BaseStat baseStat,Transform target,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        projectile.Init(baseStat, _originPoolDic[poolIndex] as ProjectileData);    // todo UsePool 에서 한번만 하게 해놓기
        projectile.Spawn(position, target);

        return projectile;

    }
    
    /// <summary>
    /// 공격력과 방향을 받는 스폰
    /// </summary>
    public BaseProjectile Spawn(ProjectileDataIndex poolIndex, BaseStat baseStat, Vector3 dir,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        projectile.Init(baseStat, _originPoolDic[poolIndex] as ProjectileData);
        projectile.Spawn(position, dir);

        return projectile;
    }
    
    /// <summary>
    /// 공격력과 목표지점을 받는 스폰
    /// </summary>
    public BaseProjectile SpawnToTarget(ProjectileDataIndex poolIndex, BaseStat baseStat, Vector3 targetPos,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        projectile.Init(baseStat, _originPoolDic[poolIndex] as ProjectileData);
        projectile.Spawn(position, targetPos);

        return projectile;
    }

    
    /// <summary>
    /// 스킬과 타겟을 받는 스폰
    /// </summary>
    public PlayerProjectile Spawn(ProjectileDataIndex poolIndex, ActiveSkill skillStat, Transform target,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        PlayerProjectile projectile = SpawnObject<PlayerProjectile>(poolIndex, position, rotation, parent);
        if (projectile == null) return projectile;
        
        projectile.Init(skillStat, _originPoolDic[poolIndex]);
        projectile.Spawn(position, target);
        
        return projectile;
    } 
    
    /// <summary>
    /// 스킬과 방향
    /// </summary>
    public PlayerProjectile Spawn(ProjectileDataIndex poolIndex, ActiveSkill skillStat, Vector3 dir,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        PlayerProjectile projectile = SpawnObject<PlayerProjectile>(poolIndex, position, rotation, parent);
        if (projectile == null) return projectile;
        
        projectile.Init(skillStat, _originPoolDic[poolIndex]);
        projectile.Spawn(position, dir) ;

        return projectile;
    } 
    
        
    /// <summary>
    /// 스킬과 목표지점
    /// </summary>
    public PlayerProjectile SpawnToTarget(ProjectileDataIndex poolIndex, ActiveSkill skillStat, Vector3 targetPos,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        PlayerProjectile projectile = SpawnObject<PlayerProjectile>(poolIndex, position, rotation, parent);
        if (projectile == null) return projectile;

        projectile.Init(skillStat, _originPoolDic[poolIndex]);
        projectile.Spawn(position, targetPos);

        return projectile;
    } 


}
