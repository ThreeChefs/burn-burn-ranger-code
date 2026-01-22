using UnityEngine;

[ExecuteAlways]
public class IgnoreSafeArea : MonoBehaviour
{
    RectTransform _rt;
    Canvas _rootCanvas;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _rootCanvas = GetComponentInParent<Canvas>();
        Apply();
    }

    void OnRectTransformDimensionsChange()
    {
        Apply();
    
    }
    

    void Apply()
    {
        if (_rt == null) return;
        if (_rootCanvas == null) _rootCanvas = GetComponentInParent<Canvas>();
        if (_rootCanvas == null) return;

        RectTransform canvasRect = _rootCanvas.GetComponent<RectTransform>();
        if (canvasRect == null) return;

        Vector2 size = canvasRect.rect.size;

        _rt.anchorMin = new Vector2(0.5f, 0.5f);
        _rt.anchorMax = new Vector2(0.5f, 0.5f);
        _rt.pivot = new Vector2(0.5f, 0.5f);

        _rt.anchoredPosition = Vector2.zero;
        _rt.sizeDelta = size;
    }
}