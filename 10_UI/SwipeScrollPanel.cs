using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class  SwipeScrollPanel : MonoBehaviour
                                        , IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] protected RectTransform _originContentPrefab;
    [SerializeField] private Transform _pivot;
    
    [Title("Scroll Options")]
    //[SerializeField] bool _isVertical;      // 아직 없음!
    [SerializeField] float _stopVelocity  = 200;
    [SerializeField] float _maxScale = 1.1f;
    [SerializeField] float _minScale = 1.0f;
    [SerializeField] float _scaleRange = 300f;
    
    ScrollRect _scrollRect;
    LayoutGroup _layoutGroup;
    protected RectTransform _contentsRect;
    
    protected List<RectTransform> _contents = new List<RectTransform>();
    
    protected int _nowContentNum = 0;
    protected bool _isDragging = false;
    protected bool _isSnapping = true;
    
    float _snapDuration = 0.2f;
    
    public event Action OnDragStartAction;
    public event Action OnDragEndAction;

    public event Action<int> OnSnapAction;


    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _layoutGroup = GetComponentInChildren<LayoutGroup>();
        if (_layoutGroup != null)
        {
            _contentsRect = _layoutGroup.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if(_scrollRect == null) return;
        
        Vector2 scrollVelocity = _scrollRect.velocity;
        float velocity = scrollVelocity.magnitude;
        
        if (_isDragging || velocity > _stopVelocity)
        {
            UpdateContentScale();
        }

        if (_isDragging == false && velocity <= _stopVelocity
            && _isSnapping == false )
        {
            _scrollRect.velocity = Vector2.zero;
            
            _isSnapping = true;
            Snap();
        }
        
        UpdateContentScale();
    }
    
    private void UpdateContentScale()
    {
        float pivotX = _pivot.position.x;

        for (int i = 0; i < _contents.Count; i++)
        {
            float distance = Mathf.Abs(pivotX - _contents[i].position.x);

            float t = Mathf.Clamp01(distance / _scaleRange);
            float scale = Mathf.Lerp(_maxScale, _minScale, t);

            _contents[i].localScale = Vector3.one * scale;
        }
    }
    private Tween _snapTween;
    protected void Snap()
    {
        float pivotX = _pivot.position.x;
        
        int closeIndex = 0;
        float minDistance = Mathf.Abs(pivotX - _contents[0].position.x);
        
        for (int i = 1; i < _contents.Count; i++)
        {
            float distance = Mathf.Abs(pivotX - _contents[i].position.x);

            if (distance < minDistance)
            {
                minDistance = distance;
                closeIndex  = i;
            }
            else
            {
                break;
            }
        }
        
        _nowContentNum = closeIndex;

        float deltaX = pivotX - _contents[_nowContentNum].position.x;

        Vector3 pos = _contentsRect.localPosition;
        pos.x += deltaX;

        _snapTween = _contentsRect.DOLocalMoveX(pos.x, _snapDuration, true)
                                  .OnComplete(OnSnap);

    }

    void OnSnap()
    {
        OnSnapAction?.Invoke(_nowContentNum);
    }
    
    private void OnDragStart()
    {
        OnDragStartAction?.Invoke();
    }

    private void OnDragEnd()
    {
        OnDragEndAction?.Invoke();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _snapTween?.Kill();
        
        _isDragging = true;
        _isSnapping = false;
        OnDragStart();
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        OnDragEnd();
    }
    
    private void Reset()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
