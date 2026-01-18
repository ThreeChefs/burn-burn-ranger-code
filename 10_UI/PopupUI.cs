using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PopupUI : BaseUI
{
    [Title("Popup UI Settings")]
    [SerializeField] PopupUIOpenType _openType = PopupUIOpenType.Default;
    [SerializeField] PopupUIOpenType _closeType = PopupUIOpenType.Default;

    float _popupDuration = 0.25f;

    protected override void AwakeInternal()
    {
    }

    public override void OpenUIInternal()
    {
        // 자식 찾아서 애니메이션 주기
        Transform transform = this.transform.GetChild(0);


        switch (_openType)
        {
            case PopupUIOpenType.None:

                break;

            case PopupUIOpenType.Default:
                if (transform != null)
                {
                    transform.localScale = Vector3.zero;
                    transform.DOScale(1f, _popupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Horizontal:
                if (transform != null)
                {
                    transform.localScale = new Vector3(0, 1, 1);
                    transform.DOScale(1f, _popupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Vertical:
                if (transform != null)
                {
                    transform.localScale = new Vector3(1, 0, 1);
                    transform.DOScale(1f, _popupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;
        }

    }

    public override Tween CloseUIInternal()
    {
        // 자식 찾아서 애니메이션 주기
        Transform transform = this.transform.GetChild(0);



        switch (_closeType)
        {
            case PopupUIOpenType.Default:
                if (transform != null)
                {
                    return transform.DOScale(0f, _popupDuration).SetEase(Ease.InQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Horizontal:
                if (transform != null)
                {
                    return transform.DOScale(new Vector3(0, 1, 1), _popupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Vertical:
                if (transform != null)
                {
                    return transform.DOScale(new Vector3(1, 0, 1), _popupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;
        }


        return null;
    }


}

public enum PopupUIOpenType
{
    None,
    Default,
    Horizontal,
    Vertical,
}
