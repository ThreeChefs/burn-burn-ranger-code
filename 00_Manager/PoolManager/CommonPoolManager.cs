using UnityEngine;

public class CommonPoolManager : PoolManager<CommonPoolManager, CommonPoolIndex>
{


    protected override void Init()
    {
        base.Init();

    }

    public override void UsePool(CommonPoolIndex poolIndex)
    {
        if (nowPoolDic.ContainsKey(poolIndex)) return;
        if (_originPoolDic.ContainsKey(poolIndex) == false) return;

        PoolObject originPrefab = _originPoolDic[poolIndex].OriginPrefab;

        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(originPrefab, 10);    // todo : default size 지정 필요
        newPool.name = $"{poolIndex}_Pool";

        nowPoolDic.Add(poolIndex, newPool);
    }


    public GameObject Spawn(CommonPoolIndex poolIndex, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        return SpawnObject(poolIndex, position, rotation, parent);
    }

    public T Spawn<T>(CommonPoolIndex poolIndex, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        GameObject go = SpawnObject(poolIndex, position, rotation, parent);
        if (go == null) return default;

        T t = go.GetComponent<T>();
        return t;
    }

}
