using DG.Tweening;
using UnityEngine;

public class AlphaFadeout : PoolObject
{
    [SerializeField] SpriteRenderer _spr;
    public void SetDuration(float duration)
    {
        Color color = _spr.color;
        color.a = 1f;
        _spr.color = color;

        Color _targetColor = color;
        _targetColor.a = 0f;

        _spr.DOColor(_targetColor, duration).OnComplete(OnColorTweenEnd);
    }

    void OnColorTweenEnd()
    {
        this.gameObject.SetActive(false);
    }

    protected override void OnDisableInternal()
    {
        DOTween.Kill(_spr);
    }

}
