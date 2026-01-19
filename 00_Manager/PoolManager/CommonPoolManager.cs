using UnityEngine;

public class CommonPoolManager : PoolManager<CommonPoolManager, CommonPoolIndex>
{


    protected override void Init()
    {
        base.Init();

    }

    public override bool UsePool(CommonPoolIndex poolIndex)
    {
        if (nowPoolDic.ContainsKey(poolIndex)) return false;
        if (_originPoolDic.ContainsKey(poolIndex) == false) return false;

        PoolObject originPrefab = _originPoolDic[poolIndex].OriginPrefab;

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(_originPoolDic[poolIndex]);
        newPool.name = $"{poolIndex}_Pool";

        nowPoolDic.Add(poolIndex, newPool);
        return true;
    }


    public PoolObject Spawn(CommonPoolIndex poolIndex, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        return SpawnObject(poolIndex, position, rotation, parent);
    }

    public T Spawn<T>(CommonPoolIndex poolIndex, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        GameObject go = SpawnObject(poolIndex, position, rotation, parent).gameObject;
        if (go == null) return default;

        T t = go.GetComponent<T>();
        return t;
    }

}
