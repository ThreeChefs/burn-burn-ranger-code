using UnityEngine;

public class Kunai : PlayerProjectile
{
    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);
        targetDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected override void MoveAndRotate()
    {
        Move(targetDir);
    }

    protected override void Move(Vector2 dir)
    {
        Vector3 targetPos = speed * Time.fixedDeltaTime * dir;
        transform.position += targetPos;
    }
}
