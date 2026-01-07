using System;
using System.Collections.Generic;

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

    public override void UsePool(ProjectileDataIndex dataIndex)
    {
        if (_projectileDataDic.ContainsKey(dataIndex) == false) return;
        if (nowPoolDic.ContainsKey(dataIndex)) return;
        
        ProjectileData data = _projectileDataDic[dataIndex];

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(data.OriginPrefab, data.DefaultPoolSize);
        nowPoolDic.Add(dataIndex, newPool);
    }
}
