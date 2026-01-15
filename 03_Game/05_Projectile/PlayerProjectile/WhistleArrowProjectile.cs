using DG.Tweening;
using UnityEngine;

public class WhistleArrowProjectile : PlayerProjectile
{
    private bool _hasHitTarget = false;

    private Tween _rotateTween;
    private float _rotationDuration;

    public override void Init(ActiveSkill activeSkill, PoolObjectData originData)
    {
        base.Init(activeSkill, originData);

        var data = originData as WhistleArrowProjectileData;
        _rotationDuration = data.RotationDuration;
    }

    protected override void HandleHit(Collider2D collision)
    {
        if (collision.transform == target)
        {
            _hasHitTarget = true;
        }

        base.HandleHit(collision);
    }

    protected override void SetGuidance()
    {
        if (!_hasHitTarget && !target.gameObject.activeSelf || _hasHitTarget)
        {
            target = MonsterManager.Instance.GetRandomMonster();
            if (target == null)
            {
                moveDir = Vector2.zero;
                return;
            }
            _hasHitTarget = false;

            if ((target.position - transform.position).normalized == moveDir)
            {
                _rotateTween?.Kill();
                _rotateTween = null;
            }
            StartRotate();

            moveDir = transform.right;    // 진행 방향으로 유지
        }
    }

    private void StartRotate()
    {
        if (_rotateTween != null && _rotateTween.IsActive()) return;

        _rotateTween = transform
            .DORotate(new Vector3(0, 0, 360), _rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }
}
