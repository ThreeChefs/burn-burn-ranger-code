using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasePool : MonoBehaviour
{
    protected PoolObjectData poolObjectData;

    protected HashSet<PoolObject> activatedObjectsPool;
    protected HashSet<PoolObject> deactivatedObjectsPool;

    public HashSet<PoolObject> ActivatedObjectsPool => activatedObjectsPool;

    protected PoolObject originPrefab;
    protected int nowPoolSize = 0;

    public event Action<PoolObject> OnActivateAction;
    public event Action<PoolObject> OnDeactivateAction;

    public void Init(PoolObjectData poolObjectData)
    {  
        this.poolObjectData = poolObjectData;
        originPrefab = poolObjectData.OriginPrefab;
        
        activatedObjectsPool = new HashSet<PoolObject>();
        deactivatedObjectsPool = new HashSet<PoolObject>();
        
        for (int i = 0; i < poolObjectData.DefaultPoolSize; i++)
        {
            PoolObject newPoolObject = CreateGameObject();
            GameObject newObject = newPoolObject.gameObject;
            newObject.transform.SetParent(this.gameObject.transform,true);

            OnDeactivateAction?.Invoke(newPoolObject);
        }
    }

    protected virtual PoolObject CreateGameObject()
    {
        PoolObject newGameObject = Instantiate(originPrefab);
        newGameObject.gameObject.SetActive(false);
        
        newGameObject.gameObject.name = nowPoolSize.ToString();
        nowPoolSize++;

        newGameObject.InitPoolObject();
        deactivatedObjectsPool.Add(newGameObject);
        
        // PoolObject 가 Disable 될 때 
        newGameObject.OnDisableAction += OnDeactivatePoolObject;
        newGameObject.OnDestroyAction += OnDestroyPoolObject;

        return newGameObject;
    }

    /// <summary>
    /// PoolManager 에서 Spawn 할 때 접근해서 쓰고 있음
    /// </summary>
    /// <returns></returns>
    public PoolObject GetPoolObject()
    {
        foreach (PoolObject poolObject in deactivatedObjectsPool)
        {
            if (poolObject.gameObject.activeInHierarchy == false)
            {
                ActivateGameObject(poolObject);
                return poolObject;
            }
        }
        
        PoolObject newObject = CreateGameObject();
        newObject.gameObject.SetActive(true);
        ActivateGameObject(newObject);
        
        return newObject;
    }

    /// <summary>
    /// PoolObject가 OnDestroy 되었다면
    /// </summary>
    protected void OnDestroyPoolObject(PoolObject poolObject)
    {
        activatedObjectsPool.Remove(poolObject);
        deactivatedObjectsPool.Remove(poolObject);
        
        OnDeactivateAction?.Invoke(poolObject);
    }

    protected void OnDeactivatePoolObject(PoolObject poolObject)
    {
        activatedObjectsPool.Remove(poolObject);
        deactivatedObjectsPool.Add(poolObject);

        OnDeactivateAction?.Invoke(poolObject);
    }

    void ActivateGameObject(PoolObject poolObject)
    {
        poolObject.gameObject.SetActive(true);
        
        deactivatedObjectsPool.Remove(poolObject);
        activatedObjectsPool.Add(poolObject);

        OnActivateAction?.Invoke(poolObject);
    }


    public void DeactivateAllPoolObjects()
    {
        PoolObject[] activatedArray = activatedObjectsPool.ToArray();

        for (int i = 0; i < activatedArray.Length; i++)
        {
            activatedArray[i].gameObject.SetActive(false);
        }

    }

    
}