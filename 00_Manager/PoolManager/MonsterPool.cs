using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : BasePool
{

    protected override PoolObject CreateGameObject()
    {
        PoolObject newGameObject = Instantiate(originPrefab);
        newGameObject.gameObject.SetActive(false);
        
        newGameObject.gameObject.name = nowPoolSize.ToString();
        nowPoolSize++;
        
        deactivatedObjectsPool.Add(newGameObject);
        
        // PoolObject 가 Disable 될 때 
        newGameObject.OnDisableAction += OnDeactivatePoolObject;


        // 몬스터 세팅
        Monster monster = newGameObject.GetComponent<Monster>();
        if(monster != null)
        {
            MonsterPoolObjectData monsterData = poolObjectData as MonsterPoolObjectData;
            monster.ApplyData(monsterData.MonsterData);
            monster.onDieAction += StageManager.Instance.OnDieMonster;
            monster.onDieAction += DeativateMonster;
        }


        return newGameObject;
    }

    void DeativateMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    public void KillAll()
    {
        // 반대로

        foreach(Monster monster in activatedObjectsPool)
        {
            monster.BombDie();
        }

        //for(int i = ActivatedObjectsPool.Count -1; i >=0; i--)
        //{
        //    Monster monster = ActivatedObjectsPool[i] as Monster;
        //    if (monster != null)
        //    {
        //        monster.BombDie();
        //    }
        //}
    }

}
