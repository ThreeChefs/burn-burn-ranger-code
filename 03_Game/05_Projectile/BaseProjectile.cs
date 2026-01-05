using UnityEngine;

public class BaseProjectile : BasePool
{
    protected ProjectileType type;
    protected Vector2 offset;

    public virtual void Init(ProjectileType type)
    {
        this.type = type;
    }

    public virtual void Spawn(Vector2 pos)
    {
        transform.position = pos + offset;
    }
}
