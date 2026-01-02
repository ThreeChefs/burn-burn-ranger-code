using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PoolManager : GlobalSingletonManager<PoolManager>
{
    [SerializeField] List<BasePool> poolsOrigin;    // Pool Prefabs. poolOriginDic에 초기화 할 때에만 사용!
    
    Dictionary<PoolType, BasePool> poolOriginDic;   // Key 로 담아둔 Origin Pool 들. Instantiate 해서 사용해야함
    Dictionary<PoolType, BasePool> nowPoolDic;      // Scene 에서 사용할 Pool 들을 Instantiate 하고 넣어둘 Dictionary.

    protected override void Init()
    {
        poolOriginDic = ((PoolType[])Enum.GetValues(typeof(PoolType))).ToDictionary(part => part,
            part => (BasePool)null);

        for (int i = 0; i < poolsOrigin.Count; i++)
        {
            if (Enum.TryParse(poolsOrigin[i].name, true, out PoolType poolType))
            {
                poolOriginDic[poolType] = poolsOrigin[i];
            }
        }
        
        nowPoolDic =  new Dictionary<PoolType, BasePool>();
        
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }


    /// <summary>
    /// Scene에서 사용할 Pool 들 사용하겠다고 알려줘야해요
    /// </summary>
    /// <param name="poolType"></param>
    public void UsePool(PoolType poolType)
    {
        if (poolOriginDic.ContainsKey(poolType))
        {
            BasePool newPool = Instantiate(poolOriginDic[poolType]);
            newPool.Init();
            nowPoolDic[poolType] = newPool;
        }
    }

    public GameObject Spawn(PoolType poolType, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (nowPoolDic.ContainsKey(poolType))
        {
            if (nowPoolDic[poolType] == null) return null;

            GameObject newGameObject = nowPoolDic[poolType].GetGameObject();

            newGameObject.transform.position = position;
            newGameObject.transform.rotation = rotation;


            if (parent != null)
            {
                newGameObject.transform.SetParent(parent);
            }

            return newGameObject;
        }

        return null;
    }

    public GameObject Spawn(PoolType poolType)
    {
        if (nowPoolDic.ContainsKey(poolType))
        {
            GameObject newGameObject = nowPoolDic[poolType].GetGameObject();
            return newGameObject;
        }

        return null;
    }

    public void DeactivateAllPoolObjects(PoolType poolType)
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