using System.Collections;
using UnityEngine;

public class BossNormalPattern3 : BossPatternBase
{
    [Header("Aim Shot")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private int shotCount = 5;        // 총 5발
    [SerializeField] private float shotInterval = 2f;  // 2초 간격
    [SerializeField] private float projectileSpeed = 10f;

    [SerializeField] private float minDistanceToRun = 0.5f; // 너무 붙었을 때 발사 방지(선택)

    protected override bool CanRun()
    {
        if (projectilePrefab == null) { Debug.LogWarning("[P3] projectilePrefab NULL", this); return false; }
        if (firePoint == null) { Debug.LogWarning("[P3] firePoint NULL", this); return false; }
        if (boss == null) { Debug.LogWarning("[P3] boss NULL", this); return false; }
        if (boss.target == null) { Debug.LogWarning("[P3] boss.target NULL", this); return false; }



        return true;
    }

    protected override IEnumerator Execute()
    {



        for (int i = 0; i < shotCount; i++)
        {
            if (boss == null || boss.IsDead) yield break;
            if (boss.Target == null) yield break;

            Vector3 spawnPos = firePoint.position;

            Vector3 toTarget = boss.Target.position - spawnPos;
            float dist = toTarget.magnitude;
            if (dist <= minDistanceToRun)
            {
                // 너무 붙었으면 발사 스킵(원하면 그냥 break 해도 됨)
                if (i < shotCount - 1)
                    yield return new WaitForSeconds(shotInterval);
                continue;
            }

            Vector2 dir = (toTarget / dist); // normalized


            var proj = ProjectileManager.Instance.Spawn(
                ProjectileDataIndex.DragonProjectile,
                boss.Attack, // BaseStat
                dir,
                spawnPos,
                Quaternion.identity,
                parent: null
            );

            if (proj != null && proj.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.velocity = dir * projectileSpeed;
            }

            if (i < shotCount - 1)
                yield return new WaitForSeconds(shotInterval);
        }

    }

}
