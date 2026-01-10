using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class JoyStickInput : MonoBehaviour
{
    [Header("조이스틱 이미지")]
    [SerializeField] private RectTransform _joyStickBase;
    [SerializeField] private RectTransform _joyStickKnob;
    [SerializeField] private float _radiusMargin;

    // 컴포넌트
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Camera _camera;

    // 인풋 관리
    private bool _inputActive;
    private Vector2 _inputStartPos;
    private float _radiusOffset;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _radiusOffset = (_joyStickBase.rect.width / 2);
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchScreenPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartInput(touchScreenPos);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    UpdateInput(touchScreenPos);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndInput();
                    break;
            }
        }
    }

    private void StartInput(Vector2 touchScreenPos)
    {
        _inputActive = true;
    }

    private void UpdateInput(Vector2 touchScreenPos)
    {
        if (_inputActive)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                _inputStartPos,
                _camera,
                out Vector2 canvasPos);

            Vector2 inputVector = touchScreenPos - canvasPos;
            Vector2 localInputVector = inputVector / _canvas.scaleFactor;

            Vector2 clampedOffset = Vector2.ClampMagnitude(localInputVector, _radiusOffset);

            _joyStickKnob.localPosition = clampedOffset;
        }
    }

    private void EndInput()
    {
        _inputActive = false;
        _joyStickKnob.localPosition = Vector2.zero;
    }

    #region 에디터 전용
#if UNITY_EDITOR
    private void Reset()
    {
        _joyStickBase = transform.FindChild<RectTransform>("Image - JoyStickBase");
        _joyStickKnob = transform.FindChild<RectTransform>("Image - JoyStickKnob");
    }
#endif
    #endregion
}
