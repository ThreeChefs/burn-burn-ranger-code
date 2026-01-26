using Sirenix.OdinInspector;
using UnityEngine;

public class SafeAreaRect : MonoBehaviour
{
    [FoldoutGroup("세로모드 추가 여백")][SerializeField] private float _portraitPadLeft = 0f;
    [FoldoutGroup("세로모드 추가 여백")][SerializeField] private float _portraitPadRight = 0f;
    [FoldoutGroup("세로모드 추가 여백")][SerializeField] private float _portraitPadTop = 0f;
    [FoldoutGroup("세로모드 추가 여백")][SerializeField] private float _portraitPadBottom = 0f;

    [FoldoutGroup("가로모드 추가 여백")][SerializeField] private float _landscapePadLeft = 0f;
    [FoldoutGroup("가로모드 추가 여백")][SerializeField] private float _landscapePadRight = 0f;
    [FoldoutGroup("가로모드 추가 여백")][SerializeField] private float _landscapePadTop = 0f;
    [FoldoutGroup("가로모드 추가 여백")][SerializeField] private float _landscapePadBottom = 0f;

    private RectTransform _panel;

    private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
    private Vector2 _minAnchor = Vector2.zero;
    private Vector2 _maxAnchor = Vector2.one;


    private void Awake()
    {
        _panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void OnEnable()
    {
        ApplySafeArea();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (_panel == null) return;

        Rect safeArea = Screen.safeArea;

        float padLeft;
        float padRight;
        float padTop;
        float padBottom;

        bool isLandscape = Screen.width > Screen.height;

        if (isLandscape)
        {
            padLeft = _landscapePadLeft;
            padRight = _landscapePadRight;
            padTop = _landscapePadTop;
            padBottom = _landscapePadBottom;
        }
        else
        {
            padLeft = _portraitPadLeft;
            padRight = _portraitPadRight;
            padTop = _portraitPadTop;
            padBottom = _portraitPadBottom;
        }

        safeArea.xMin += padLeft;
        safeArea.xMax -= padRight;
        safeArea.yMin += padBottom;
        safeArea.yMax -= padTop;

        if (safeArea.xMin < 0f) safeArea.xMin = 0f;
        if (safeArea.yMin < 0f) safeArea.yMin = 0f;
        if (safeArea.xMax > Screen.width) safeArea.xMax = Screen.width;
        if (safeArea.yMax > Screen.height) safeArea.yMax = Screen.height;

        if (safeArea.width < 0f) safeArea.width = 0f;
        if (safeArea.height < 0f) safeArea.height = 0f;

        if (safeArea == _lastSafeArea) return;
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