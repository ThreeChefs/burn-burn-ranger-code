using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DimmedUI : BaseUI
{
    CanvasGroup _canvasGroup;
    Stack<BaseUI> calledUI = new();

    protected override void AwakeInternal()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetDimmed(PopupUI target)
    {
        if (target == null) return;

        if (calledUI.Count <= 0)
        {
            FadeInDim();
        }

        SetSiblingOrder(target);

        calledUI.Push(target);
        target.OnCloseAction += CloseUI;
    }


    void SetSiblingOrder(PopupUI target)
    {
        if(target == null)  return;

        Transform targetParent = target.transform.parent;
        if (targetParent == null) return;

        transform.SetParent(targetParent, false);

        transform.SetSiblingIndex(target.transform.GetSiblingIndex());
    }

    

    void FadeInDim()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1f, PopupUI.PopupDuration).SetUpdate(true);
    }

    public void CloseUI(BaseUI target)
    {
        // target 상관 없이 어차피 쌓이는 대로 close가 불리는 구조
        if (calledUI.Peek() == target)  // 그래도 확인은하자..?
        {
            target.OnCloseAction -= CloseUI;

            PopupUI prevPopup = (PopupUI)calledUI.Pop();
            if (prevPopup != null && calledUI.Count>0)
            {
                SetSiblingOrder((PopupUI)calledUI.Peek());
            }
        }

        if (calledUI.Count == 0)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.DOFade(0f, PopupUI.PopupDuration).SetUpdate(true).OnComplete(CloseUI);
        }
    }




}
