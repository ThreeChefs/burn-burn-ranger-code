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
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector2 dir = Random.insideUnitCircle.normalized;
       
        
        //Vector3 randomPos = player.transform.position + (Vector3)(dir * Define.RandomRange(Define.MinMonsterSpawnDistance, Define.MaxMonsterSpawnDistance));
        
        PoolObject monsterPoolObject = SpawnObject(poolIndex,
            player.transform.position + new Vector3(dir.x*camWidth, dir.y * camHeight,0));

        // todo : Monster가 PoolObject로부터 상속받도록 변경 필요 또는 캐싱하고 있기
        if(monsterPoolObject == null) return  null;

        Monster monster = monsterPoolObject as Monster;
        monster.ApplyData(((MonsterPoolObjectData)_originPoolDic[poolIndex]).MonsterData);
        return monster;

    }

    public Monster SpawnBossMonster(MonsterPoolIndex poolIndex)
    {
        DeactiveAllMonsters();

        return SpawnWaveMonster(poolIndex);
    }


    /// <summary>
    /// 그냥 없애기만 하는거
    /// </summary>
    public void DeactiveAllMonsters()
    {
        if (nowPoolDic.Count == 0) return;

        foreach (var pool in nowPoolDic.Values)
        {
            pool.DeactivateAllPoolObjects();
        }
    }


    public void KillAll()
    {
        if (nowPoolDic.Count == 0) return;
        foreach (var pool in nowPoolDic.Values)
        {
            MonsterPool monsterPool =  pool as MonsterPool;
            if(monsterPool != null)
            {
                monsterPool.KillAll();
            }
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
                Monster monster = obj as Monster;

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
