using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    Canvas _canvas;

    public RectTransform SafeArea => _panel;

    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
    private Vector2 _minAnchor = Vector2.zero;
    private Vector2 _maxAnchor = Vector2.one;


    void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {

        ApplySafeArea();
    }

    public void SetSortingOrder(int sortingOrder)
    {
        _canvas.sortingOrder = sortingOrder;
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