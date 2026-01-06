using UnityEngine;

public class Kunai : PlayerProjectile
{
    public override void Spawn(Transform target)
    {
        base.Spawn(target);
        targetDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        data = AssetLoader.FindAndLoadByName<ProjectileData>("KunaiProjectileData");
    }
#endif
}
