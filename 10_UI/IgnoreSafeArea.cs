using UnityEngine;

public class IgnoreSafeArea : MonoBehaviour
{
    RectTransform _rt;
    Canvas _canvas;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        Apply();
    }

    

    void Apply()
    {
        if (_rt == null) return;
        if (_canvas == null) _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null) return;

        RectTransform rootRect = _canvas.rootCanvas.GetComponent<RectTransform>();
        if (rootRect == null) return;

        Vector2 size = rootRect.rect.size;

        _rt.anchorMin = new Vector2(0.5f, 0.5f);
        _rt.anchorMax = new Vector2(0.5f, 0.5f);
        _rt.pivot = new Vector2(0.5f, 0.5f);

        _rt.anchoredPosition = Vector2.zero;
        _rt.sizeDelta = size;
    }
}