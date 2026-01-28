using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : BasePool
{

    protected override PoolObject CreateGameObject()
    {

        PoolObject newGameObject = base.CreateGameObject();

        // 몬스터 세팅
        Monster monster = (Monster)newGameObject;
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

 
}
