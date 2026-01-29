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

    /// <summary>
    /// 특정 콜라이더에 충돌 시 반사 처리
    /// </summary>
    /// <param name="collision"></param>
    public void OnHit(Collider2D collision)
    {
        Vector2 norm = Vector2.zero;

        if (collision.gameObject.layer == Define.WallLayer)
        {
            Vector2 hitPos = collision.ClosestPoint(_self.position);
            norm = ((Vector2)_self.position - hitPos).normalized;
        }
        else if (collision.gameObject.layer == Define.MonsterLayer)
        {
            norm = (_self.position - collision.transform.position).normalized;
        }

        if (norm.sqrMagnitude < 0.0001f) return;

        _projectile.MoveDir = Vector2.Reflect(_projectile.MoveDir, norm).normalized;
        _self.position += _projectile.MoveDir * 0.05f;        // 재충돌 방지
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
