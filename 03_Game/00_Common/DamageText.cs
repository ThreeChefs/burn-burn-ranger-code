using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : PoolObject
{
    [SerializeField] TextMeshProUGUI _damageText;

    public void Init(float damage)
    {
        _damageText.SetText("{0:0}", damage);
        // 컬러 알파 초기화
        Color color = _damageText.color;
        color.a = 1f;
        _damageText.color = color;

        DOTween.ToAlpha(
            () => _damageText.color,
            color => _damageText.color = color,
            0f,
            1f
        ).SetEase(Ease.InExpo).OnComplete(() => gameObject.SetActive(false));

        DOTween.To(
            () => transform.localPosition,
            pos => transform.localPosition = pos,
            new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, transform.localPosition.z),
            1f
        ).SetEase(Ease.OutCubic);
    }



}
