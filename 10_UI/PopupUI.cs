using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : BaseUI
{
    [Title("Popup UI Settings")]
    [SerializeField] bool _useDim;
    //[ShowIf("_useDim")][SerializeField] Image _dim; 

    [SerializeField] Transform _popup;
    [SerializeField] PopupUIOpenType _openType = PopupUIOpenType.Default;
    [SerializeField] PopupUIOpenType _closeType = PopupUIOpenType.Default;

    static public float PopupDuration = 0.25f;

    protected override void AwakeInternal()
    {
    }

    public override void OpenUIInternal()
    {
        // 자식 찾아서 애니메이션 주기
        //Transform transform = this.transform.GetChild(0);

        if (_useDim)
        {
            DimmedUI dim = (DimmedUI)UIManager.Instance.ShowUI(UIName.UI_Dimmed);
            if (dim != null)
            {
                dim.SetSiblingOrder(this);
            }
        }
       
        switch (_openType)
        {
            case PopupUIOpenType.None:

                break;

            case PopupUIOpenType.Default:
                if (_popup != null)
                {
                    _popup.localScale = Vector3.zero;
                    _popup.DOScale(1f, PopupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Horizontal:
                if (_popup != null)
                {
                    _popup.localScale = new Vector3(0, 1, 1);
                    _popup.DOScale(1f, PopupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Vertical:
                if (_popup != null)
                {
                    _popup.localScale = new Vector3(1, 0, 1);
                    _popup.DOScale(1f, PopupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;
        }

    }

    public override Tween CloseUIInternal()
    {
        // 자식 찾아서 애니메이션 주기
        //Transform transform = this.transform.GetChild(0);

        switch (_closeType)
        {
            case PopupUIOpenType.Default:
                if (_popup != null)
                {
                    return _popup.DOScale(0f, PopupDuration).SetEase(Ease.InQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Horizontal:
                if (_popup != null)
                {
                    return _popup.DOScale(new Vector3(0, 1, 1), PopupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Vertical:
                if (_popup != null)
                {
                    return _popup.DOScale(new Vector3(1, 0, 1), PopupDuration).SetEase(Ease.OutQuad).SetUpdate(true);
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
