using UnityEngine;

public class Kunai : BaseProjectile
{
    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);
        transform.rotation = Quaternion.LookRotation(target.position);
    }

    protected override void Move(Vector2 dir)
    {
        Vector3 targetPos = speed * Time.fixedDeltaTime * dir;
        transform.position += targetPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            Attack(damageable);
            passCount--;
            if (passCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
