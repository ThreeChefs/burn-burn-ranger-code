using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterManager : PoolManager<MonsterManager, MonsterPoolIndex>
{

    public override void UsePool(MonsterPoolIndex poolIndex)
    {
        if(nowPoolDic.ContainsKey(poolIndex)) return;
        if(_originPoolDic.ContainsKey(poolIndex) == false) return;

        MonsterPoolObjectData data= (MonsterPoolObjectData)_originPoolDic[poolIndex];
        
        if (data == null) return;
        if (data.OriginPrefab == null) return;
        
        BasePool newPool = Instantiate(poolPrefab);
        newPool.Init(_originPoolDic[poolIndex]);
        newPool.name = $"{poolIndex}_Pool";
        nowPoolDic.Add(poolIndex, newPool);
    }


    public Monster SpawnWaveMonster(MonsterPoolIndex poolIndex)
    {

        StagePlayer player = PlayerManager.Instance.StagePlayer;
        Vector2 dir = Random.insideUnitCircle;
        dir.Normalize();

        Vector3 randomPos = player.transform.position + (Vector3)(dir * Define.RandomRange(Define.MinMonsterSpawnDistance, Define.MaxMonsterSpawnDistance));
        
        PoolObject monsterPoolObject = SpawnObject(poolIndex,randomPos);

        // todo : Monster가 PoolObject로부터 상속받도록 변경 필요 또는 캐싱하고 있기
        Monster monster = monsterPoolObject.GetComponent<Monster>();
        monster.ApplyData(((MonsterPoolObjectData)_originPoolDic[poolIndex]).MonsterData);
        return monster;

        //Monster monster = SpawnObject<Monster>(poolIndex, position, rotation, parent);
        //return monster;
    }

    public Monster SpawnBossMonster(MonsterPoolIndex poolIndex)
    {
        DeactiveAllMonsters();
        return null;
    }


    public void DeactiveAllMonsters()
    {
        if (nowPoolDic.Count == 0) return;

        foreach (var pool in nowPoolDic.Values)
        {
            // todo : 펑 터뜨리기
            pool.DeactivateAllPoolObjects();
        }
    }

    public void AllKill()
    {
        if (nowPoolDic.Count == 0) return;
        foreach (var pool in nowPoolDic.Values)
        {
            // todo : 펑 터뜨리기
            pool.SendMessageToActivated("Die");
        }
    }


    public Transform GetNearestMonster()
    {
        Monster nearestMonster = null;
        StagePlayer player = PlayerManager.Instance.StagePlayer;

        foreach (var pool in nowPoolDic.Values)
        {
            foreach (var obj in pool.ActivatedObjectsPool)
            {
                Monster monster = obj.GetComponent<Monster>();

                if (monster == null) continue;
                if (nearestMonster == null)
                {
                    nearestMonster = monster;
                    continue;
                }

                float currentDistance = Vector2.Distance(monster.transform.position, player.transform.position);
                float nearestDistance = Vector2.Distance(nearestMonster.transform.position, player.transform.position);

                if (currentDistance < nearestDistance)
                {
                    nearestMonster = monster;
                }
            }
        }

        return nearestMonster != null ? nearestMonster.transform : null;
    }
}
