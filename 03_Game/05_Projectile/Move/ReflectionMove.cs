using UnityEngine;

/// <summary>
/// 투사체 이동 - 반사
/// </summary>
public class ReflectionMove : IProjectileMove
{
    private readonly BaseProjectile _projectile;
    private readonly Transform _self;

    private readonly IProjectileMove _baseMove;

    private readonly LayerMask _targetLayer;
    private readonly Camera _cam;

    public ReflectionMove(
        BaseProjectile projectile,
        IProjectileMove baseMove,
        LayerMask targetLayer)
    {
        _projectile = projectile;
        _self = _projectile.transform;
        _baseMove = baseMove;
        _targetLayer = targetLayer;

        _cam = Camera.main;
    }

    public void MoveAndRotate(float deltaTime)
    {
        _baseMove.MoveAndRotate(deltaTime);
        HandleScreenReflection();
    }

    private void HandleScreenReflection()
    {
        if (((1 << Define.WallLayer) & _targetLayer) == 0) return;

        Vector2 pos = _self.position;
        Vector2 dir = _projectile.MoveDir;
        Vector2 camPos = _cam.transform.position;

        float halfH = _cam.orthographicSize;
        float halfW = halfH * _cam.aspect;

        float minX = camPos.x - halfW;
        float maxX = camPos.x + halfW;
        float minY = camPos.y - halfH;
        float maxY = camPos.y + halfH;

        bool reflected = false;

        if (pos.x < minX || pos.x > maxX)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            dir.x *= -1;
            reflected = true;
        }

        if (pos.y < minY || pos.y > maxY)
        {
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            dir.y *= -1;
            reflected = true;
        }

        if (reflected)
        {
            _projectile.MoveDir = dir;
            _self.position = pos;
            _self.rotation = Quaternion.Euler(
                0f,
                0f,
                Mathf.Atan2(_projectile.MoveDir.y, _projectile.MoveDir.x) * Mathf.Rad2Deg);
            _projectile.PlaySfxOnce();
        }
    }
}
