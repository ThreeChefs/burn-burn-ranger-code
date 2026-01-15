using UnityEngine;

/// <summary>
/// 투사체 - 드릴샷
/// </summary>
public class DrillShotPlayerProjectile : PlayerProjectile
{
    protected override void HandleScreenReflection()
    {
        base.HandleScreenReflection();

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg);
    }
}
