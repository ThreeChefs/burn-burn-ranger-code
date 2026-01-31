using DG.Tweening;
using UnityEngine;

public class AlphaFadeout : PoolObject
{
    [SerializeField] SpriteRenderer _spr;
    public void SetDuration(float duration, float rate = 0)
    {
        Color color = _spr.color;
        color.a = 1f;
        _spr.color = color;

        Color _targetColor = color;
        _targetColor.a = 0f;

        float delay = duration * rate;
        float fadeDuration = duration * (1 - rate);

        _spr.DOColor(_targetColor, fadeDuration).SetDelay(delay).OnComplete(OnColorTweenEnd);
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
