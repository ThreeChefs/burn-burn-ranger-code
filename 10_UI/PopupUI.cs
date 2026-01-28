using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PopupUI : BaseUI
{
    static public float PopupDuration = 0.25f;

    [BoxGroup("Popup UI Settings")][SerializeField] bool _useDim;
    [ReadOnly, SerializeField][BoxGroup("Popup UI Settings")] CanvasGroup _canvasGroup;

    PopupUIElement[] _popupElements;

    //[BoxGroup("Popup UI Settings")][SerializeField] Transform _popup;
    //[BoxGroup("Popup UI Settings")][SerializeField] PopupUIOpenType _openType = PopupUIOpenType.Default;
    //[BoxGroup("Popup UI Settings")][SerializeField] PopupUIOpenType _closeType = PopupUIOpenType.Default;


    protected override void AwakeInternal()
    {
        _popupElements = transform.GetComponentsInChildren<PopupUIElement>(true);
        _canvasGroup = GetComponent<CanvasGroup>();
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
                dim.SetDimmed(this);
            }
        }

        _canvasGroup.interactable = false;

        if (_popupElements != null && _popupElements.Length > 0)
        {
            _popupElements?.ForEach(e => e.Open(PopupDuration));
            DOVirtual.DelayedCall(PopupDuration,
                () => { _canvasGroup.interactable = true; })
                .SetUpdate(true);
        }
        else
        {
            _canvasGroup.interactable = false;
        }


    }

    public override Tween CloseUIInternal()
    {
        // 자식 찾아서 애니메이션 주기
        //Transform transform = this.transform.GetChild(0);

        _canvasGroup.interactable = false;

        if (_popupElements != null && _popupElements.Length > 0)
        {
            _popupElements?.ForEach(e => e.Close(PopupDuration));
            return DOVirtual.DelayedCall(PopupDuration, null).SetUpdate(true);
        }

        // 팝업 애니메이션 요소 없으면 바로 꺼지기
        return null;
    }

    private void OnValidate()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
}

public enum PopupUIOpenType
{
    Default,    // 그냥 커지기
    Horizontal, // 수평으로 커지기
    Vertical,   // 수직으로 커지기
    MoveRight,  // 화면밖에서 오른쪽으로 움직여서 나오기 / Close 할 때는 오른쪽 화면 밖으로 나가기
    MoveLeft,   // 화면 밖에서 왼쪽으로 움직여서 나오기 / Close 할 때는 왼쪽 화면 밖으로 나가기
    MoveTop,
    MoveBottom,
}
