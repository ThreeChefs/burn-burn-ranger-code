using DG.Tweening;
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
            transform.DOScale(1f, 0.25f).SetEase(Ease.OutQuad).OnComplete(AnimationEnd);
        }

    }

    private void AnimationEnd()
    {
        // 애니메이션 끝났을 때 처리
        // 인터랙션 가능하게 한다던가
    }
}
