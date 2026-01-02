using System.Collections.Generic;
using UnityEngine;

public class BasePool : MonoBehaviour
{
    private List<PoolObject> activatedObjectsPool;
    List<PoolObject> deactivatedObjectsPool;
    
    [SerializeField] private PoolObject prefab;
    [SerializeField] private int defaultPoolSize = 50;

    private int nowPoolSize = 0;
    public void Init()
    {  
        activatedObjectsPool = new List<PoolObject>();
        deactivatedObjectsPool = new List<PoolObject>();
        
        for (int i = 0; i < defaultPoolSize; i++)
        {
            GameObject newObject = CreateGameObject().gameObject;
            newObject.transform.SetParent(this.gameObject.transform,true);
        }
    }

    private PoolObject CreateGameObject()
    {
        PoolObject newGameObject = Instantiate(prefab);
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
    public GameObject GetGameObject()
    {
        foreach (PoolObject poolObject in deactivatedObjectsPool)
        {
            if (poolObject.gameObject.activeInHierarchy == false)
            {
                ActivateGameObject(poolObject);
                return poolObject.gameObject;
            }
        }
        
        PoolObject newObject = CreateGameObject();
        newObject.gameObject.SetActive(true);
        ActivateGameObject(newObject);
        
        return newObject.gameObject;
    }

    void OnDisableAction(PoolObject poolObject)
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

    
}