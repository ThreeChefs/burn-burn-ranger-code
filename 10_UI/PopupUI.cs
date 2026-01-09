using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : BaseUI
{
    public override void OpenUIInternal()
    {

        // 자식 찾아서 애니메이션 주기
        Transform transform = this.transform.GetChild(0);
        
        if(transform != null)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.25f).SetEase(Ease.OutQuad);
        }

    }
}
