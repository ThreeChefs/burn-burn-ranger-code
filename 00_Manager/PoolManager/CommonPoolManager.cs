using UnityEngine;

public class CommonPoolManager : PoolManager<CommonPoolManager, CommonPoolIndex>
{
    // 간단한 스폰 함수들
    public PoolObject Spawn(CommonPoolIndex poolIndex,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        return SpawnObject(poolIndex, position, rotation, parent);
    }

    // 스폰 후 특정 컴포넌트 반환
    public T Spawn<T>(CommonPoolIndex poolIndex,
        Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        GameObject go = SpawnObject(poolIndex, position, rotation, parent).gameObject;
        if (go == null) return default;

        T t = go.GetComponent<T>();
        return t;
    }
}
