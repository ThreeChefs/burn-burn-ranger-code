using System;
using System.Collections.Generic;

public class ProjectileManager : PoolManager<ProjectileManager,ProjectilePoolIndex>
{
    Dictionary<ProjectilePoolIndex,ProjectileData> _projectileDataDic = new Dictionary<ProjectilePoolIndex, ProjectileData>();
    
    protected override void Init()
    {
        base.Init();
        
        List<ProjectileData> _projectileDatas = poolDatabase.GetDatabase<ProjectileData>();

        foreach (ProjectileData data in _projectileDatas)
        {
            if (Enum.TryParse( data.name, true, out ProjectilePoolIndex poolIndex))
            {
                _projectileDataDic[poolIndex] = data;
            }
        }
    }

    public override void UsePool(ProjectilePoolIndex poolIndex)
    {
        if (_projectileDataDic.ContainsKey(poolIndex) == false) return;
        if (nowPoolDic.ContainsKey(poolIndex)) return;
        
        ProjectileData data = _projectileDataDic[poolIndex];

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(data.OriginPrefab, data.DefaultPoolSize);
        nowPoolDic.Add(poolIndex, newPool);
    }
}
