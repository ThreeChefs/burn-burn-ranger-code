using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FortuneBoxButton : BaseButton
{
    public event Action _buttonAction;

    protected override void Awake()
    {
        base.Awake();
        _button.onClick.AddListener(OnClickButton);
    }

    public void OnClickButton()
    {
        _buttonAction?.Invoke();
        _buttonAction = null;

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
       
    }

}
