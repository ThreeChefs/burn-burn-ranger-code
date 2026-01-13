using System.Collections.Generic;
using UnityEngine;

public class GuardianBladeController : ActiveSkill
{
    [Header("Orbit")]
    [SerializeField] private float radius = 1.2f;

    [SerializeField] private float rotationSpeed = 180f;

    [Header("Blade Count Rule")]
    [SerializeField] private int startCount = 2;
    [SerializeField] private int maxCount = 6;

    [Header("Data")]
    [SerializeField] ProjectileData projectileData; //SO 에설정한 값들 그대로 가져옴 

    // 런타임 참조
    private Transform owner;
    private BaseStat attackStat;

    // 배치용
    private int index;
    private int totalCount;
    private float angle;


    private float tickTimer;
    private readonly HashSet<IDamageable> targets = new();


    public void Init(
        Transform owner,
        BaseStat attack,
        int index,
        int totalCount,
        ProjectileData data
    )
    {
        this.owner = owner;
        this.attackStat = attack;
        this.index = index;
        this.totalCount = Mathf.Max(1, totalCount);
        this.projectileData = data;

        // 처음부터 균등 배치
        angle = 360f / this.totalCount * index;
    }

    protected virtual void Update()
    {
        if (owner == null || projectileData == null)
            return;


        angle += rotationSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset =
            new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;

        transform.position = owner.position + offset;


        if (targets.Count == 0)
            return;

        tickTimer += Time.deltaTime;
        //  if (tickTimer < projectileData.TickInterval)
        return;

        tickTimer = 0f;

        // float damage =
        //     attackStat.MaxValue * projectileData.DamageMultiplier;

        //   foreach (var t in targets)
        {
            //      t?.TakeDamage(damage);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (projectileData == null)
            return;


        if (((1 << other.gameObject.layer) &
             projectileData.TargetLayerMask) == 0)
            return;

        if (other.TryGetComponent<IDamageable>(out var d))
            targets.Add(d);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var d))
            targets.Remove(d);
    }

    private void OnDisable()
    {
        targets.Clear();
        tickTimer = 0f;
    }

}
