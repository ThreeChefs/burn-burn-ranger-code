using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlphaFadeout : PoolObject
{
    [SerializeField] SpriteRenderer _spr;
    float _duration = 0.2f;

    protected override void OnEnableInternal()
    {
        Color color = _spr.color;
        color.a = 1f;
        _spr.color = color;

        Color _targetColor = color;
        _targetColor.a = 0f;

        _spr.DOColor(_targetColor, _duration).OnComplete(OnColorTweenEnd);

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
