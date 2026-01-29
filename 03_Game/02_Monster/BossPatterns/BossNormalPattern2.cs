using System.Collections;
using UnityEngine;

public class BossNormalPattern2 : BossPatternBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 8f;

    [Header("발사각도")]
    [SerializeField] private float startAngle = 0f;
    [SerializeField] private float stepAngle = 10f;
    [SerializeField] private float shotInterval = 0.03f;
    [SerializeField] private int loops = 1;


    protected override bool CanRun()
    {

        if (projectilePrefab == null)
        {
            return false;
        }
        return true;
    }

    protected override IEnumerator Execute()
    {


        int shotsPerLoop = Mathf.CeilToInt(360f / Mathf.Max(0.01f, stepAngle));
        float angle = startAngle;

        for (int l = 0; l < loops; l++)
        {
            for (int i = 0; i < shotsPerLoop; i++)
            {
                Fire(angle);
                angle += stepAngle; // 시계방향으로 회전 (반시계로 하고 싶으면 -=)
                yield return new WaitForSeconds(shotInterval);
            }
        }
    }
    private void Fire(float angleDeg)
    {
        Vector3 pos = firePoint != null ? firePoint.position : transform.position;

        float rad = angleDeg * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        var proj = ProjectileManager.Instance.Spawn(
            ProjectileDataIndex.DragonProjectile,
            boss.Attack,
            dir,
            pos,
            Quaternion.Euler(0f, 0f, angleDeg)
        );

        if (proj == null) return;

        // 🔹 이동을 BaseProjectile이 안 한다면 여기서 보완
        if (proj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.velocity = dir * projectileSpeed;
        }
    }
}
