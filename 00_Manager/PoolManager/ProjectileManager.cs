using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : PoolManager<ProjectileManager, ProjectileDataIndex>
{
    protected override void Init()
    {
        base.Init();
    }

    public override void UsePool(ProjectileDataIndex dataIndex)
    {
        if (nowPoolDic.ContainsKey(dataIndex)) return;
        if (_originPoolDic.ContainsKey(dataIndex) == false) return;

        ProjectileData data = (ProjectileData)_originPoolDic[dataIndex];

        if (data == null) return;
        if (data.OriginPrefab == null) return;

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(_originPoolDic[dataIndex]);
        newPool.name = $"{dataIndex}_Pool";

        nowPoolDic.Add(dataIndex, newPool);
    }

    public void UsePool(ProjectileDataIndex dataIndex, ActiveSkillData skillData)
    {

    }

    // todo :  Spawn 할 때 projectile Init 필요

    public BaseProjectile Spawn(ProjectileDataIndex poolIndex, BaseStat baseStat, Transform target, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolIndex, position, rotation, parent);

        if (projectile == null) return projectile;

        projectile.Init(baseStat, _originPoolDic[poolIndex] as ProjectileData);    // todo UsePool 에서 한번만 하게 해놓기
        projectile.Spawn(position, target);

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
            projectile.Init(baseStat, skillData);
        }

        projectile.Spawn(position, target);

        return projectile;

    }



}
