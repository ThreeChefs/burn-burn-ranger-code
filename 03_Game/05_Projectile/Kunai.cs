using UnityEngine;

public class Kunai : BaseProjectile
{
    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);
        Vector2 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected override void Move(Vector2 dir)
    {
        Vector3 targetPos = speed * Time.fixedDeltaTime * dir;
        transform.position += targetPos;
    }
}
