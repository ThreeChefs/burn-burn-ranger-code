using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : PoolManager<ProjectileManager, ProjectileDataIndex>
{
    protected override void Init()
    {
        base.Init();
    }

    public override bool UsePool(ProjectileDataIndex dataIndex)
    {
        if (nowPoolDic.ContainsKey(dataIndex)) return false;
        if (_originPoolDic.ContainsKey(dataIndex) == false) return false;

        ProjectileData data = (ProjectileData)_originPoolDic[dataIndex];

        if (data == null) return false ;
        if (data.OriginPrefab == null) return false;

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(_originPoolDic[dataIndex]);
        newPool.name = $"{dataIndex}_Pool";

        nowPoolDic.Add(dataIndex, newPool);

        return true;
    }

    public void UsePool(ProjectileDataIndex dataIndex, ActiveSkillData skillData)
    {

    }

    // todo :  Spawn 할 때 projectile Init 필요
    
    /// <summary>
    /// 공격력과 타겟을 받는 스폰
    /// </summary>
    public BaseProjectile Spawn(ProjectileDataIndex poolIndex, BaseStat baseStat, Transform target, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
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
    public BaseProjectile Spawn(ProjectileDataIndex poolIndex, BaseStat baseStat, Vector3 dir, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        projectile.Init(baseStat, _originPoolDic[poolIndex] as ProjectileData);

        return projectile;
    }
    
    /// <summary>
    /// 공격력과 목표지점을 받는 스폰
    /// </summary>
    public BaseProjectile SpawnToTarget(ProjectileDataIndex poolIndex, BaseStat baseStat, Vector3 targetPos, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        projectile.Init(baseStat, _originPoolDic[poolIndex] as ProjectileData);

        return projectile;
    }

    
    /// <summary>
    /// 스킬과 타겟을 받는 스폰
    /// </summary>
    public PlayerProjectile Spawn(ProjectileDataIndex poolIndex, ActiveSkill skillStat, Transform target, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        PlayerProjectile projectile = SpawnObject<PlayerProjectile>(poolIndex, position, rotation, parent);
        if (projectile == null) return projectile;
        
        projectile.Init(skillStat, _originPoolDic[poolIndex]);
        projectile.Spawn(position, target);
        
        return projectile;
    } 
    
    /// <summary>
    /// 스킬과 목표지점을 받는 스폰
    /// </summary>
    public PlayerProjectile Spawn(ProjectileDataIndex poolIndex, ActiveSkill skillStat, Vector3 dir, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        PlayerProjectile projectile = SpawnObject<PlayerProjectile>(poolIndex, position, rotation, parent);
        if (projectile == null) return projectile;
        
        projectile.Init(skillStat, _originPoolDic[poolIndex]);

        return projectile;
    } 
    
        
    /// <summary>
    /// 공격력과 방향을 받는 스폰
    /// </summary>
    public PlayerProjectile SpawnToTarget(ProjectileDataIndex poolIndex, ActiveSkill skillStat, Vector3 targetPos, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        PlayerProjectile projectile = SpawnObject<PlayerProjectile>(poolIndex, position, rotation, parent);
        if (projectile == null) return projectile;

        return projectile;
    } 

    
    

    // 플레이어만 임시사용
    public BaseProjectile Spawn(ProjectileDataIndex poolIndex, BaseStat baseStat, Transform target, ActiveSkillData skillData, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        PlayerProjectile playerProjectile = (PlayerProjectile)projectile;

        // todo: 임시. 나중에 UsePool 할 때 써야해
        if (playerProjectile != null)
        {
            //projectile.Init(baseStat, skillData);
        }

        projectile.Spawn(position, target);

        return projectile;

    }



}
