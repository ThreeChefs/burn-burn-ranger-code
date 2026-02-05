using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropBox : Monster
{
    [Serializable]
    public class DropEntry
    {
        public GameObject prefab;
        [Min(0f)] public float weight = 1f;
    }

    [Header("Item Drop Line")]
    [SerializeField] private List<DropEntry> drops = new();
    [SerializeField] private bool allowDropNothing = false;
    [SerializeField] private float nothingWeight = 1f;

    [Header("Drop Position")]
    [SerializeField] private Transform dropPoint;

    protected override void OnEnableInternal()
    {
        allowFlip = false;      // ★ 상자는 뒤집지 않음
        base.OnEnableInternal();
    }
    protected override void DropItem()
    {
        Vector3 pos = (dropPoint != null) ? dropPoint.position : transform.position;
        SpawnRandom(pos);
    }
    private void SpawnRandom(Vector3 pos)
    {
        if (drops == null || drops.Count == 0) return;

        float total = 0f;
        if (allowDropNothing) total += Mathf.Max(0f, nothingWeight);

        for (int i = 0; i < drops.Count; i++)
        {
            var e = drops[i];
            if (e.prefab == null) continue;
            if (e.weight <= 0f) continue;
            total += e.weight;
        }

        if (total <= 0f) return;

        float r = UnityEngine.Random.value * total;

        // 아무것도 안 나오는 옵션
        if (allowDropNothing)
        {
            float nw = Mathf.Max(0f, nothingWeight);
            if (r < nw) return;
            r -= nw;
        }

        // 가중치 랜덤 선택
        for (int i = 0; i < drops.Count; i++)
        {
            var e = drops[i];
            if (e.prefab == null || e.weight <= 0f) continue;

            if (r < e.weight)
            {
                Instantiate(e.prefab, pos, Quaternion.identity);
                return;
            }
            r -= e.weight;
        }
    }
}
