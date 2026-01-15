using DG.Tweening;
using UnityEngine;

public class WhistleArrowProjectile : PlayerProjectile
{
    private bool _hasHitTarget = false;

    private Tween _rotateTween;
    private Tween _findTargetTween;
    private float _rotationDuration = 0.5f;
    private float _findTargetInterval = 0.5f;

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
        if (_hasHitTarget)
        {
            target = null;
            StartRotate();
            StartFindTarget();
            moveDir = transform.right;    // 진행 방향으로 유지
            return;
        }

        base.SetGuidance();
    }

    private void StartRotate()
    {
        if (_rotateTween != null && _rotateTween.IsActive()) return;

        _rotateTween = transform
            .DORotate(new Vector3(0, 0, 360), _rotationDuration, RotateMode.LocalAxisAdd)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }

    private void StartFindTarget()
    {
        if (_findTargetTween != null && _findTargetTween.IsActive()) return;

        _findTargetTween = DOVirtual.DelayedCall(_findTargetInterval, FindTarget).SetLoops(-1);
    }

    private void FindTarget()
    {
        target = MonsterManager.Instance.GetRandomMonster();

        if (target == null) return;

        _hasHitTarget = false;

        _rotateTween?.Kill();
        _findTargetTween?.Kill();

        _rotateTween = null;
        _findTargetTween = null;
    }
}
