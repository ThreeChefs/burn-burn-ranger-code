using DG.Tweening;
using UnityEngine;

public class InGameLoadingUI : BaseUI
{
    [SerializeField] private RectTransform _bg;
    [SerializeField] private float _delay = 0.1f;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _maxScale = 10f;
    [SerializeField] private float _minScale = 1f;

    private Tween _tween;

    private void OnDestroy()
    {
        _tween?.Kill();
    }

    public void StartAnim(bool expandMode)
    {
        if (expandMode)
        {
            _bg.localScale = Vector2.one * _minScale;
            _tween = _bg.DOScale(_maxScale, _duration).SetDelay(_delay).OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            _bg.localScale = Vector3.one * _maxScale;
            _tween = _bg.DOScale(_minScale, _duration).SetDelay(_delay);
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _bg = transform.FindChild<RectTransform>("Image");
    }
#endif
}
