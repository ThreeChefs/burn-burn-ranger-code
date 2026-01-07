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


    public GameObject Spawn(ProjectileDataIndex poolType, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        Logger.Log("다른 함수 사용!!!!!!!!!! 매개변수가 추가됐서용");
        return null;
    }
    
    public BaseProjectile Spawn<T>(ProjectileDataIndex poolType, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        Logger.Log("다른 함수 사용!!!!!!!!!! / 매개변수가 추개됐어용");
        return null;
    }

    public BaseProjectile Spawn(ProjectileDataIndex poolType, BaseStat baseStat, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolType, position, rotation, parent);

        if (projectile == null) return projectile;
        
        projectile.Init(baseStat, _projectileDataDic[poolType]);
        
        return projectile;
        
    }
    
    public BaseProjectile Spawn(ProjectileDataIndex poolType, BaseStat baseStat, ActiveSkillData skillData, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        BaseProjectile projectile = SpawnObject<BaseProjectile>(poolType, position, rotation, parent);

        if (projectile == null) return projectile;

        PlayerProjectile playerProjectile = (PlayerProjectile)projectile;

        // todo: 임시. 나중에 UsePool 할 때 써야해
        if (playerProjectile != null)
        {
            projectile.Init(baseStat, skillData);

        }
        else
        {
            projectile.Init(baseStat, _projectileDataDic[poolType]);
        }
        
        return projectile;
        
    }

    

}
