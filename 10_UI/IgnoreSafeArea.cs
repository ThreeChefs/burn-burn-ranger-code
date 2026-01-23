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
        if (_canvas == null) return;

        _rt.anchorMin = Vector2.zero;
        _rt.anchorMax = Vector2.one;
        _rt.pivot = new Vector2(0.5f, 0.5f);

        _rt.anchoredPosition = Vector2.zero;
        _rt.sizeDelta = Vector2.zero;
        _rt.offsetMin = Vector2.zero;
        _rt.offsetMax = Vector2.zero;
    }
}