using DG.Tweening;
using UnityEngine;

public class InGameLoadingUI : BaseUI
{
    [SerializeField] private RectTransform _bg;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _startScale = 10f;
    [SerializeField] private float _endScale = 1f;

    public override void OpenUIInternal()
    {
        base.OpenUIInternal();

        _bg.localScale = Vector3.one * _startScale;
        _bg.DOScale(_endScale, _duration);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _bg = transform.FindChild<RectTransform>("Image");
    }
#endif
}
