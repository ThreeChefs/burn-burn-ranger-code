using UnityEngine;

public class SafeAreaRect : MonoBehaviour
{
    private RectTransform _panel;

    public RectTransform SafeArea => _panel;

    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
    private Vector2 _minAnchor = Vector2.zero;
    private Vector2 _maxAnchor = Vector2.one;


    void Awake()
    {
        _panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }



    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        if (safeArea != _lastSafeArea)
        {
            _lastSafeArea = safeArea;

            _minAnchor = safeArea.position;
            _maxAnchor = safeArea.position + safeArea.size;

            _minAnchor.x /= Screen.width;
            _minAnchor.y /= Screen.height;
            _maxAnchor.x /= Screen.width;
            _maxAnchor.y /= Screen.height;

            _panel.anchorMin = _minAnchor;
            _panel.anchorMax = _maxAnchor;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }

}