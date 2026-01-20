using UnityEngine;

public class SoccerBallActiveSkill : ActiveSkill
{
    protected override PlayerProjectile SpawnProjectile()
    {
        return ProjectileManager.Instance.Spawn(
            projectileIndex,
            this,
            new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized,
            new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f)));
    }
}
