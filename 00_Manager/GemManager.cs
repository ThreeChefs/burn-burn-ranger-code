using System.Collections.Generic;
using UnityEngine;

public class GemManager : PoolManager<GemManager, GemPoolIndex>
{
    [SerializeField] private Transform gemRoot;
    public override bool UsePool(GemPoolIndex poolIndex)
    {
        if (nowPoolDic.ContainsKey(poolIndex) && nowPoolDic[poolIndex] != null)
        {
            return true;
        }

        if (!_originPoolDic.TryGetValue(poolIndex, out var data))
        {
            return false;
        }

        BasePool pool = Instantiate(poolPrefab, gemRoot != null ? gemRoot : transform);
        pool.Init(data);
        nowPoolDic[poolIndex] = pool;
        return true;
    }

    public void MagnetGems(Transform target)
    {

        if (nowPoolDic == null || nowPoolDic.Count == 0)
        {
            return;
        }
        foreach (var kv in nowPoolDic)
        {
            var pool = kv.Value;
            if (pool == null) continue;
            // HashSet이라 순회 중 비활성화되면 위험 -> 스냅샷 복사
            var snapshot = new List<PoolObject>(pool.ActivatedObjectsPool);
            for (int i = 0; i < snapshot.Count; i++)
            {
                if (snapshot[i] == null) continue;
                if (!snapshot[i].gameObject.activeInHierarchy) continue;

                if (snapshot[i].TryGetComponent<GemItem>(out var gem))
                {

                    gem.StartMagnet(target);
                }
            }
        }
    }
    public void CollectAllGemsInstant(StagePlayer player)
    {
        foreach (var kv in nowPoolDic)
        {
            var pool = kv.Value;
            if (pool == null) continue;

            var snapshot = new List<PoolObject>(pool.ActivatedObjectsPool);
            for (int i = 0; i < snapshot.Count; i++)
            {
                var po = snapshot[i];
                if (po == null) continue;
                if (!po.gameObject.activeInHierarchy) continue;

                if (po.TryGetComponent<GemItem>(out var gem))
                {
                    player.StageLevel.AddExp(gem.ExpValue);
                    po.gameObject.SetActive(false); // Destroy X, 풀 반납
                }
            }
        }
    }
    public GemItem SpawnGem(GemPoolIndex type, Vector3 position, int dropCount)
    {
        return SpawnObject<GemItem>(type, position);
    }

}
