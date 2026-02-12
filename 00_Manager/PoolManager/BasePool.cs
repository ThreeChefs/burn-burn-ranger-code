using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePool : MonoBehaviour
{
    protected PoolObjectData poolObjectData;        // 이 Pool 이 어떤 PoolObjectData 를 사용하는지 저장 

    protected HashSet<PoolObject> activatedObjectsPool;     // 활성화된 오브젝트들
    protected HashSet<PoolObject> deactivatedObjectsPool;   // 비활성화된 오브젝트들

    public HashSet<PoolObject> ActivatedObjectsPool => activatedObjectsPool;

    protected PoolObject originPrefab;              // 이 Pool 이 생성할 오브젝트의 원본 프리팹
    protected int nowPoolSize = 0;                  // 현재 Pool 에 생성된 오브젝트 수

    public event Action<PoolObject> OnActivateAction;   // PoolObject 가 활성화 될 때 호출되는 이벤트
    public event Action<PoolObject> OnDeactivateAction; // PoolObject 가 비활성화 될 때 호출되는 이벤트

    // PoolObjectData 로 Pool 초기화
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

    // 새로운 PoolObject 생성
    protected virtual PoolObject CreateGameObject()
    {
        PoolObject newGameObject = Instantiate(originPrefab);
        newGameObject.gameObject.SetActive(false);
        
        newGameObject.gameObject.name = nowPoolSize.ToString();
        nowPoolSize++;

        newGameObject.InitPoolObject();
        deactivatedObjectsPool.Add(newGameObject);
        
        // PoolObject 가 Disable 될 때 호출되는 이벤트 등록
        newGameObject.OnDisableAction += OnDeactivatePoolObject;
        newGameObject.OnDestroyAction += OnDestroyPoolObject;

        return newGameObject;
    }

    // PoolObject 하나 가져오기
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

    // Pool 내 모든 오브젝트 비활성화
    public void DeactivateAllPoolObjects()
    {   
        // 이벤트로 activatedObjectsPool 에서 제거되니 하나씩 반복문 돌리기
        while (activatedObjectsPool.Count > 0)
        {
            PoolObject target = null;

            foreach (PoolObject poolObject in activatedObjectsPool)
            {
                target = poolObject;
                break;
            }

            if (target == null)
            {
                break;
            }

            target.gameObject.SetActive(false);
        }
    }

    
}