using UnityEngine;

public class StraightMove : IProjectileMove
{
    private readonly BaseProjectile _projectile;
    private readonly Transform _self;

    public StraightMove(BaseProjectile projectile)
    {
        _projectile = projectile;
        _self = projectile.transform;

        Vector3 moveDir = _projectile.MoveDir;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        _self.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void MoveAndRotate(float deltaTime)
    {
        Vector3 targetPos = _projectile.Speed * deltaTime * _projectile.MoveDir;
        _self.position += targetPos;
    }
}
