using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : PoolObject
{
    [SerializeField] TextMeshProUGUI _damageText;

    public void Init(int damage)
    {
        _damageText.text = damage.ToString();

        DOTween.ToAlpha(
            () => _damageText.color,
            color => _damageText.color = color,
            0f,
            1f
        ).SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false));
    }



}
