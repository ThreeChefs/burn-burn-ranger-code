
using System.Collections;
using UnityEngine;
public class RangedAttackMonster : Monster
{
    [Header("Projectile")]
    [SerializeField] private BaseProjectile projectilePrefab;  //BaseProjectile을 가져온이유: 스크립트를 읽어보니 공통 프로젝타일들의 로직인것같아서 
    [SerializeField] private Transform firePoint; //몬스터가 쏘는위치
    [SerializeField] private ProjectileData projectileData;
    [Header("Detect / Fire")]
    [SerializeField] private float detectRange = 7f;
    [SerializeField] private float fireInterval = 0.3f;
    [SerializeField] private int fireCount = 3;

    private bool _isFiring;

    protected override void FixedUpdate()
    {

        if (target == null) return;

        // 발사 중이면 이동 자체를 막아놓음
        if (_isFiring)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // 평소엔 추격임 
        base.FixedUpdate();

        float dist = Vector2.Distance(transform.position, target.transform.position);
        if (dist <= detectRange)
        {
            StartCoroutine(FireBurstRoutine());
        }
    }

    private IEnumerator FireBurstRoutine()
    {
        _isFiring = true;

        // 공격시 멈추기  코루틴썻음 ㅇㅇ  공격시 멈췃다가 0.3초동안 3발 발사하고 fireinterval 기다리고 다시 추격 
        if (rb != null)
            rb.velocity = Vector2.zero;

        for (int i = 0; i < fireCount; i++)
        {
            if (target == null)
                break;

            Fire();
            yield return new WaitForSeconds(fireInterval);
        }

        _isFiring = false;
    }

    private void Fire()
    {
        Debug.Log($"Fire() prefab={(projectilePrefab != null)} firePoint={(firePoint != null)} target={(target != null)} projData={(projectileData != null)}");
        if (projectilePrefab == null) return;
        if (target == null) return;

        Vector2 spawnPos = firePoint ? (Vector2)firePoint.position : (Vector2)transform.position;
        BaseProjectile proj = ProjectileManager.Instance.Spawn<BaseProjectile>(ProjectileDataIndex.RangedAttack, spawnPos);
        proj.Spawn(firePoint.position, target.transform);

    }
}
