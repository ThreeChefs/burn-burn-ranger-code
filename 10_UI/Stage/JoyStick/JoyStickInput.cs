using UnityEngine;

public class JoyStickInput : BaseUI
{
    [Header("조이스틱 이미지")]
    [SerializeField] private RectTransform _outerCircle;
    [SerializeField] private RectTransform _innerCircle;
    [SerializeField] private float _radiusMargin;

    // 컴포넌트
    private RectTransform _rectTransform;
    private Camera _camera;

    // 인풋 관리
    private bool _inputActive;
    private Vector2 _inputStartPos;
    private float _radiusOffset;

    private Vector2 _defaultPos;

    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _radiusOffset = (_outerCircle.rect.width / 2);
        _defaultPos = transform.position;
    }

    private void Start()
    {
        _camera = Camera.main;
        //if (!Application.isMobilePlatform)
        //{
        //    gameObject.SetActive(false);
        //}
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
        _inputStartPos = touchScreenPos;

        // 동적 조이스틱
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            touchScreenPos,
            _camera,
            out Vector2 localPos);

        _outerCircle.localPosition = localPos;
        _innerCircle.localPosition = Vector2.zero;
    }

    private void UpdateInput(Vector2 touchScreenPos)
    {
        if (!_inputActive) return;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            touchScreenPos,
            _camera,
            out Vector2 currentLocalPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            _inputStartPos,
            _camera,
            out Vector2 startLocalPos);

        Vector2 delta = currentLocalPos - startLocalPos;
        Vector2 clamped = Vector2.ClampMagnitude(delta, _radiusOffset);

        _innerCircle.localPosition = clamped;

        Direction = clamped / _radiusOffset;
    }

    private void EndInput()
    {
        _inputActive = false;
        Direction = Vector2.zero;
        _innerCircle.localPosition = _defaultPos;
    }

    #region 에디터 전용
#if UNITY_EDITOR
    private void Reset()
    {
        _outerCircle = transform.FindChild<RectTransform>("Image - OuterCircle");
        _innerCircle = transform.FindChild<RectTransform>("Image - InnerCircle");
    }
#endif
    #endregion
}
