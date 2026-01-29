using UnityEngine;

/// <summary>
/// 투사체 이동 - 유도
/// </summary>
public class GudianceMove : IProjectileMove
{
    private readonly BaseProjectile _projectile;
    private readonly Transform _self;

    private readonly IProjectileMove _baseMove;

    private float _gudianceTime;
    private readonly float _turnSpeed;

    public GudianceMove(
        BaseProjectile projectile,
        IProjectileMove baseMove,
        float gudianceTime,
        float turnSpeed = 1f)
    {
        _projectile = projectile;
        _self = _projectile.transform;
        _baseMove = baseMove;
        _gudianceTime = gudianceTime;
        _turnSpeed = turnSpeed;
    }

    public void MoveAndRotate(float deltaTime)
    {
        _baseMove.MoveAndRotate(deltaTime);

        if (_gudianceTime < 0f || !IsValidTarget()) return;

        Vector3 toTarget = (_projectile.Target.position - _self.position).normalized;
        _projectile.MoveDir = Vector3.Lerp(
            _projectile.MoveDir,
            toTarget,
            _turnSpeed * deltaTime);

        float angle = Mathf.Atan2(_projectile.MoveDir.y, _projectile.MoveDir.x) * Mathf.Rad2Deg;
        _self.rotation = Quaternion.Euler(0f, 0f, angle);

        _gudianceTime -= deltaTime;
    }

    private bool IsValidTarget()
    {
        return _projectile.Target == null
            || !_projectile.Target.gameObject.activeInHierarchy;
    }
}
