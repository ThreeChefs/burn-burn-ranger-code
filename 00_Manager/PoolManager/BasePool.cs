using System.Collections.Generic;
using UnityEngine;

public class BasePool : MonoBehaviour
{
    protected PoolObjectData _poolObjectData;

    protected List<PoolObject> activatedObjectsPool;
    protected List<PoolObject> deactivatedObjectsPool;

    public List<PoolObject> ActivatedObjectsPool => activatedObjectsPool;

    protected PoolObject _originPrefab;
    protected int nowPoolSize = 0;

    public void Init(PoolObjectData poolObjectData)
    {  
        _poolObjectData = poolObjectData;
        _originPrefab = poolObjectData.OriginPrefab;
        
        activatedObjectsPool = new List<PoolObject>();
        deactivatedObjectsPool = new List<PoolObject>();
        
        for (int i = 0; i < poolObjectData.DefaultPoolSize; i++)
        {
            GameObject newObject = CreateGameObject().gameObject;
            newObject.transform.SetParent(this.gameObject.transform,true);
        }
    }

    protected virtual PoolObject CreateGameObject()
    {
        PoolObject newGameObject = Instantiate(_originPrefab);
        newGameObject.gameObject.SetActive(false);
        
        newGameObject.gameObject.name = nowPoolSize.ToString();
        nowPoolSize++;
        
        deactivatedObjectsPool.Add(newGameObject);
        
        // PoolObject 가 Disable 될 때 
        newGameObject.OnDisableAction += OnDisableAction;

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

    protected void OnDisableAction(PoolObject poolObject)
    {
        if (activatedObjectsPool.Contains(poolObject) == true)
        {
            activatedObjectsPool.Remove(poolObject);
        }

        if (deactivatedObjectsPool.Contains(poolObject) == false)
        {
            deactivatedObjectsPool.Add(poolObject);
        }
    }

    void ActivateGameObject(PoolObject poolObject)
    {
        poolObject.gameObject.SetActive(true);
        if (deactivatedObjectsPool.Contains(poolObject)==true)
        {
            deactivatedObjectsPool.Remove(poolObject);
        }
        if (activatedObjectsPool.Contains(poolObject) == false)
        {
            activatedObjectsPool.Add(poolObject);
        }
    }


    public void DeactivateAllPoolObjects()
    {
        for (int i = activatedObjectsPool.Count - 1; i >= 0; i--)
        {
            activatedObjectsPool[i].gameObject.SetActive(false);
        }
    }


    public void SendMessageToActivated(string methodName)
    {
        for (int i = activatedObjectsPool.Count - 1; i >= 0; i--)
        {
            activatedObjectsPool[i].SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }
    }

    
}