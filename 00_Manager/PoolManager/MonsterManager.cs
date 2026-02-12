using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : PoolManager<MonsterManager, MonsterPoolIndex>
{
    // 활성화/비활성화 된 몬스터들 관리용 리스트
    protected List<Monster> activatedMonsters = new();
    protected List<Monster> deactivatedMonsters = new();
    private List<Monster> _monstersInArea = new();

    public override BasePool UsePool(MonsterPoolIndex poolIndex)
    {
        BasePool newPool = base.UsePool(poolIndex);
        if(newPool == null) return null;    

        // 몬스터 풀에 활성화/비활성화/파괴 콜백 등록
        newPool.OnActivateAction += OnActivateMonster;
        newPool.OnDeactivateAction += OnDeactivateMonster;

        return newPool;
    }

    // 웨이브 몬스터 스폰
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

    // 모든 몬스터 비활성화
    public void DeactiveAllMonsters()
    {
        if (nowPoolDic.Count == 0) return;

        foreach (var pool in nowPoolDic.Values)
        {
            pool.DeactivateAllPoolObjects();
        }
    }

    // 모든 몬스터 즉사
    public void KillAll()
    {
        if (nowPoolDic.Count == 0) return;

        for (int i = activatedMonsters.Count - 1; i > 0; i--)
        {
            if (activatedMonsters[i] != null)
                activatedMonsters[i].BombDie();
        }
    }

    // 플레이어와 가장 가까운 몬스터 데려오기
    public Transform GetNearestMonster()
    {
        Monster nearestMonster = null;
        StagePlayer player = PlayerManager.Instance.StagePlayer;
        Vector3 playerPos = player.transform.position;
        float nearestDistance = float.PositiveInfinity;

        foreach (Monster monster in activatedMonsters)
        {
            if (monster == null) continue;

            float currentDistance = Vector2.Distance(monster.transform.position, playerPos);

            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestMonster = monster;
            }
        }

        return nearestMonster != null ? nearestMonster.transform : null;
    }
   
    // 화면 내 랜덤 몬스터 데려오기
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

    // 몬스터 활성화/비활성화/파괴 시 호출되는 함수들
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

    public void OnDestroyMonster(PoolObject poolObject)
    {
        Monster monster = poolObject as Monster;

        if (deactivatedMonsters.Contains(monster) == true)
        {
            deactivatedMonsters.Remove(monster);
        }

        if (activatedMonsters.Contains(monster) == true)
        {
            activatedMonsters.Remove(monster);
        }
    }


}
