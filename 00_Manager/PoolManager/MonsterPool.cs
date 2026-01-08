using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : BasePool
{

    protected override PoolObject CreateGameObject()
    {
        PoolObject newGameObject = Instantiate(_originPrefab);
        newGameObject.gameObject.SetActive(false);
        
        newGameObject.gameObject.name = nowPoolSize.ToString();
        nowPoolSize++;
        
        deactivatedObjectsPool.Add(newGameObject);
        
        // PoolObject 가 Disable 될 때 
        newGameObject.OnDisableAction += OnDisableAction;


        // 몬스터 세팅
        Monster monster = newGameObject.GetComponent<Monster>();
        if(monster != null)
        {
            MonsterPoolObjectData monsterData = _poolObjectData as MonsterPoolObjectData;
            monster.ApplyData(monsterData.MonsterData);
            monster.onDieAction += StageManager.Instance.OnDieMonster;
        }


        return newGameObject;
    }

}
