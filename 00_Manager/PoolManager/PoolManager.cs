using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// todo : 코드컨벤션 맞추기
public abstract class PoolManager<T, TEnumIndex> : GlobalSingletonManager<T>
    where T : PoolManager<T, TEnumIndex> where TEnumIndex : struct, Enum       // Enum.TryParse 쓰려면 Generic 제약 struct 필요하대
{
    [SerializeField] protected BasePool poolPrefab;             // 풀 프리팹
    [SerializeField] protected PoolObjectDatabase poolDatabase;

    protected Dictionary<TEnumIndex, PoolObjectData> _originPoolDic;  // Database 에서 어떤 PoolIndex 가 있는지 확인용 Dictionary
    protected Dictionary<TEnumIndex, BasePool> nowPoolDic;      // Scene 에서 사용할 Pool 들을 Instantiate 하고 넣어둘 Dictionary.

    protected override void Init()
    {
        nowPoolDic = new Dictionary<TEnumIndex, BasePool>();
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        _originPoolDic = new();
        foreach (PoolObjectData poolObjectData in poolDatabase.List)
        {
            if (poolObjectData.OriginPrefab == null)
                continue;

            if (Enum.TryParse(poolObjectData.name, true, out TEnumIndex poolIndex))
            {
                _originPoolDic.Add(poolIndex, poolObjectData);
            }
        }
    }

    /// <summary>
    /// Scene에서 사용할 Pool 들 사용하겠다고 알려줘야해요
    /// </summary>
    /// <param name="poolType"></param>
    public abstract bool UsePool(TEnumIndex poolIndex);


    protected PoolObject SpawnObject(TEnumIndex poolType, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        if (nowPoolDic.ContainsKey(poolType) == false)
        {
            UsePool(poolType);
            
            if (nowPoolDic[poolType] == null)
            {
                return null;
            }
        }

        PoolObject newGameObject = nowPoolDic[poolType].GetPoolObject();

        if (position != default)
            newGameObject.transform.position = position;
        else
            newGameObject.transform.position = Vector3.zero;

        if (rotation != default)
            newGameObject.transform.rotation = rotation;
        else
            newGameObject.transform.rotation = Quaternion.identity;


        if (parent != null)
        {
            newGameObject.transform.SetParent(parent);
        }
        else
        {
            newGameObject.transform.SetParent(nowPoolDic[poolType].transform);
        }

        return newGameObject;

        return null;
    }

    protected TPoolObject SpawnObject<TPoolObject>(TEnumIndex poolType, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    where TPoolObject : PoolObject
    {
        PoolObject go = SpawnObject(poolType, position, rotation, parent);

        if (go != null)
        {
            TPoolObject poolObject = go.GetComponent<TPoolObject>();
            return poolObject;
        }

        return null;
    }

    public void DeactivateAllPoolObjects(TEnumIndex poolType)
    {
        if (nowPoolDic.ContainsKey(poolType))
        {
            nowPoolDic[poolType].DeactivateAllPoolObjects();
        }
    }

    /// <summary>
    /// Scene 벗어날 때 nowPoolDic 버리고 떠나기
    /// </summary>
    /// <param name="scene"></param>
    protected override void OnSceneUnloaded(Scene scene)
    {
        nowPoolDic.Clear();
    }


}