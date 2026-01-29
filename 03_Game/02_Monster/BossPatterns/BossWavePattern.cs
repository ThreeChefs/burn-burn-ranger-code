using System.Collections;
using UnityEngine;

public class BossWavePattern : BossPatternBase
{
    [Header("Center Move")]
    [SerializeField] private Transform centerPos;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float arriveDistance = 0.05f;

    [Header("Radius")]
    [SerializeField] private float radius = 8f;

    [Header("Angle Settings")]
    [Tooltip("각도 간격 (예: 10 = 10도씩)")]
    [SerializeField] private float angleStep = 10f;

    [Tooltip("각 발사 간격 (우수수 속도)")]
    [SerializeField] private float fireInterval = 0.05f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 14f;

    protected override bool CanRun()
    {
        if (boss == null) return false;
        if (centerPos == null) return false;
        if (projectilePrefab == null) return false;

        if (radius <= 0f) return false;
        if (moveSpeed <= 0f) return false;
        if (projectileSpeed <= 0f) return false;
        if (angleStep <= 0f) return false;

        return true;
    }

    protected override IEnumerator Execute()
    {

        yield return MoveToCenter(centerPos.position);

        Vector3 center = boss.transform.position;

        // 시계 방향 (0 → 360)
        yield return FireCircle(center, clockwise: true);
        yield return FireCircle(center, clockwise: false);
    }


    private IEnumerator FireCircle(Vector3 center, bool clockwise)
    {
        if (clockwise)
        {
            for (float angle = 0f; angle < 360f; angle += angleStep)
            {
                SpawnProjectileDiameter(center, radius, angle, inward: false);
                yield return new WaitForSeconds(fireInterval);
            }
        }
        else
        {
            for (float angle = 360f; angle > 0f; angle -= angleStep)
            {
                SpawnProjectileDiameter(center, radius, angle, inward: true);
                yield return new WaitForSeconds(fireInterval);
            }
        }
    }

    private IEnumerator MoveToCenter(Vector3 targetPos)
    {
        while (Vector3.Distance(boss.transform.position, targetPos) > arriveDistance)
        {
            boss.transform.position = Vector3.MoveTowards(
                boss.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        boss.transform.position = targetPos;
    }


    private void SpawnProjectileDiameter(Vector3 center, float r, float angleDeg, bool inward)
    {
        Vector2 dir = AngleToDir(angleDeg);
        Vector3 startPos = inward
       ? center + (Vector3)(dir * r)      // 바깥 원에서
       : center - (Vector3)(dir * r);     // 반대쪽 바깥 원에서(기존)

        Vector2 moveDir = inward ? -dir : dir; // ⭐ 방향을 반대로

        float rotZ = inward ? angleDeg + 180f : angleDeg;

        ProjectileManager.Instance.Spawn(ProjectileDataIndex.DragonProjectile, boss.Attack, moveDir, startPos, Quaternion.Euler(0f, 0f, angleDeg), parent: null);

    }

    private Vector2 AngleToDir(float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }
}




