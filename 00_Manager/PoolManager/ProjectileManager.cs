using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class ProjectileManager : PoolManager<ProjectileManager,ProjectileDataIndex>
{
    Dictionary<ProjectileDataIndex,ProjectileData> _projectileDataDic = new Dictionary<ProjectileDataIndex, ProjectileData>();
    
    protected override void Init()
    {
        base.Init();
        
        List<ProjectileData> _projectileDatas = poolDatabase.GetDatabase<ProjectileData>();

        foreach (ProjectileData data in _projectileDatas)
        {
            if (Enum.TryParse( data.name, true, out ProjectileDataIndex poolIndex))
            {
                _projectileDataDic[poolIndex] = data;
            }
        }
    }

    // to
    public override void UsePool(ProjectileDataIndex dataIndex)
    {
        if (_projectileDataDic.ContainsKey(dataIndex) == false) return;
        if (nowPoolDic.ContainsKey(dataIndex)) return;
        
        ProjectileData data = _projectileDataDic[dataIndex];

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(data.OriginPrefab, data.DefaultPoolSize);
        nowPoolDic.Add(dataIndex, newPool);
    }

    public void UsePool(ProjectileDataIndex dataIndex, ActiveSkillData skillData)
    {
        
    }
    
    // todo :  Spawn 할 때 projectile Init 필요

    public BaseProjectile Spawn(ProjectileDataIndex poolType, BaseStat baseStat, Transform target, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolType, position, rotation, parent);

        if (projectile == null) return projectile;
        
        projectile.Init(baseStat, _projectileDataDic[poolType]);    // todo UsePool 에서 한번만 하게 해놓기
        projectile.Spawn(position, target);
        
        return projectile;
        
    }
    
    // 플레이어만 임시사용
    public BaseProjectile Spawn(ProjectileDataIndex poolType, BaseStat baseStat, Transform target, ActiveSkillData skillData, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolType, position, rotation, parent);

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
