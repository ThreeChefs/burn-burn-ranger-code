using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : PoolManager<MonsterManager, MonsterPoolIndex>
{
    protected List<Monster> activatedMonsters = new();
    protected List<Monster> deactivatedMonsters = new();

    private List<Monster> _monstersInArea = new();

    public override bool UsePool(MonsterPoolIndex poolIndex)
    {
        if (nowPoolDic.ContainsKey(poolIndex)) return false;
        if (_originPoolDic.ContainsKey(poolIndex) == false) return false;

        MonsterPoolObjectData data = (MonsterPoolObjectData)_originPoolDic[poolIndex];

        if (data == null) return false;
        if (data.OriginPrefab == null) return false;

        BasePool newPool = Instantiate(poolPrefab);

        newPool.OnActivateAction += OnActivateMonster;
        newPool.OnDeactivateAction += OnDeactivateMonster;

        newPool.Init(_originPoolDic[poolIndex]);
        newPool.name = $"{poolIndex}_Pool";
        nowPoolDic.Add(poolIndex, newPool);

        return true;
    }


    public Monster SpawnWaveMonster(MonsterPoolIndex poolIndex)
    {
        StagePlayer player = PlayerManager.Instance.StagePlayer;
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector2 dir = Random.insideUnitCircle.normalized;

        PoolObject monsterPoolObject = SpawnObject(poolIndex,
            player.transform.position + new Vector3(dir.x * camWidth, dir.y * camHeight, 0));

        if (monsterPoolObject == null) return null;

        Monster monster = monsterPoolObject as Monster;
        monster.ApplyData(((MonsterPoolObjectData)_originPoolDic[poolIndex]).MonsterData);
        return monster;

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

        for (int i = activatedMonsters.Count - 1; i > 0; i--)
        {
            if (activatedMonsters[i] != null)
                activatedMonsters[i].BombDie();
        }
    }


    public Transform GetNearestMonster()
    {
        Monster nearestMonster = null;
        StagePlayer player = PlayerManager.Instance.StagePlayer;


        foreach (var pool in activatedMonsters)
        {
            var monster = pool as Monster;
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

        return nearestMonster != null ? nearestMonster.transform : null;
    }


    /// <summary>
    /// 살아있는 애중 영역 안에 있는 아무나 데려오기
    /// </summary>
    /// <returns></returns>
    public Transform GetRandomMonster()
    {
        _monstersInArea.Clear();

        StagePlayer player = PlayerManager.Instance.StagePlayer;
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector3 center = player.transform.position;

        float minX = center.x - camWidth;
        float maxX = center.x + camWidth;
        float minY = center.y - camHeight;
        float maxY = center.y + camHeight;

        for (int i = 0; i < activatedMonsters.Count; i++)
        {
            Monster monster = activatedMonsters[i];
            if (monster == null) continue;

            Vector3 pos = monster.transform.position;
            if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
                continue;

            _monstersInArea.Add(monster);
        }

        if (_monstersInArea.Count == 0)
            return null;

        PoolObject randomMonster = _monstersInArea.Random();
        return randomMonster != null ? randomMonster.transform : null;
    }

    public void OnActivateMonster(PoolObject poolObject)
    {
        Monster monster = poolObject as Monster;

        if (deactivatedMonsters.Contains(monster) == true)
        {
            deactivatedMonsters.Remove(monster);
        }

        if (activatedMonsters.Contains(monster) == false)
        {
            activatedMonsters.Add(monster);
        }
    }

    public void OnDeactivateMonster(PoolObject poolObject)
    {
        Monster monster = poolObject as Monster;

        if (deactivatedMonsters.Contains(monster) == false)
        {
            deactivatedMonsters.Add(monster);
        }

        if (activatedMonsters.Contains(monster) == true)
        {
            activatedMonsters.Remove(monster);
        }
    }


}
