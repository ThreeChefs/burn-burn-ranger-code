using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PopupUI : BaseUI
{

    protected override void AwakeInternal()
    {
    }

    public override void OpenUIInternal()
    {
       // 자식 찾아서 애니메이션 주기
        Transform transform = this.transform.GetChild(0);
        
        if(transform != null)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.25f).SetEase(Ease.OutQuad).SetUpdate(true);
        }

    }

    public override Tween CloseUIInternal()
    {
        // 자식 찾아서 애니메이션 주기
        Transform transform = this.transform.GetChild(0);
        
        if(transform != null)
        {
            transform.localScale = Vector3.one;
            return transform.DOScale(0f, 0.2f).SetEase(Ease.InQuad).SetUpdate(true);
        }

        return null;
    }


}
