using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class  ScrollPanel : MonoBehaviour, IBeginDragHandler, IPointerEnterHandler
{
    [SerializeField] private RectTransform _originContentPrefab;
    
    List<RectTransform> _contents;
    
    public event Action OnDragStartAction;
    public event Action OnDragEndAction;

    protected int _nowContentNum = 0;

    public abstract void Init();
    
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
        OnDragStart();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnDragEnd();
    }
}
