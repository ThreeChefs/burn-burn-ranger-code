using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickInput : BaseUI, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("조이스틱 이미지")]
    [SerializeField] private RectTransform _outerCircle;
    [SerializeField] private RectTransform _innerCircle;
    [SerializeField] private float _radiusMargin;

    // 컴포넌트
    private RectTransform _rectTransform;

    // 인풋 관리
    private bool _inputActive;
    private float _radiusOffset;

    private Vector2 _defaultLocalPos;

    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _radiusOffset = _outerCircle.rect.width * 0.5f;
        _defaultLocalPos = _outerCircle.localPosition;
        gameObject.SetActive(Define.EnableMobileUI);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartInput(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateInput(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EndInput();
    }

    private void StartInput(Vector2 touchScreenPos)
    {
        _inputActive = true;

        // 동적 조이스틱
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            touchScreenPos,
            null,
            out Vector2 localPos);

        _outerCircle.localPosition = localPos;
        _innerCircle.localPosition = Vector2.zero;
    }

    private void UpdateInput(Vector2 touchScreenPos)
    {
        if (!_inputActive) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _outerCircle,
            touchScreenPos,
            null,
            out Vector2 localPos);

        Vector2 clamped = Vector2.ClampMagnitude(localPos, _radiusOffset);

        _innerCircle.localPosition = clamped;
        Direction = clamped / _radiusOffset;
    }

    private void EndInput()
    {
        _inputActive = false;
        Direction = Vector2.zero;
        _outerCircle.localPosition = _defaultLocalPos;
        _innerCircle.localPosition = Vector2.zero;
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
