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
            if (boss.target == null) yield break;
            Vector3 spawnPos =
              (firePoint != null) ? firePoint.position :
              (boss != null ? boss.transform.position : transform.position);
            Vector2 dir = (boss.target.position - firePoint.position).normalized;

            // 발사
            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // 너가 이미 쓰는 투사체 구조에 맞춰 아래 중 하나만 선택해서 연결하면 됨.
            // 1) Rigidbody2D 기반이면:
            if (proj.TryGetComponent<Rigidbody2D>(out var rb))
                rb.velocity = dir * projectileSpeed;

            // 2) 커스텀 Init이 있다면 예시:
            // proj.Init(dir, projectileSpeed, bossAttackStatOrDamage);



            // 마지막 샷 뒤에는 대기 안 하고 종료
            if (i < shotCount - 1)
                yield return new WaitForSeconds(shotInterval);
        }


    }

}
