using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class AutoMatchingScaler : MonoBehaviour
{
    private CanvasScaler _scaler;
    private float _baseRatio;       // 1080/1920


    private void Awake()
    {
        _scaler = GetComponent<CanvasScaler>();

        _scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        Vector2 resolution = _scaler.referenceResolution;
        _baseRatio = resolution.y > 0f ? (resolution.x / resolution.y) : 0f;


        Apply();
    }

    private void OnRectTransformDimensionsChange()  // Awake 보다 먼저 들어옴!!
    {
        Apply(); 
    }


    private void Apply()
    {
        if (_scaler == null) return;
        if (_baseRatio <= 0f) return;

        float h = Screen.height;
        if (h <= 0f) return;

        float currentRatio = Screen.width / h;

        _scaler.matchWidthOrHeight = (currentRatio < _baseRatio) ? 0f : 1f;
    }
}