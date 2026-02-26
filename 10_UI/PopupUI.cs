using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class PopupUI : BaseUI
{
    static public float DefaultPopupDuration = 0.25f;

    [BoxGroup("Popup UI Settings")][SerializeField] 
    bool _useDim;   // Dimmed UI 사용 여부
    
    [BoxGroup("Popup UI Settings")][SerializeField]
    float _popupDurationRate = 1f;      // 팝업 애니메이션 재생 속도 비율

    PopupUIElement[] _popupElements;    // 팝업 애니메이션 요소들

    protected override void AwakeInternal()
    {   
        // 팝업 애니메이션 요소들 가져오기
        _popupElements = transform.GetComponentsInChildren<PopupUIElement>(true);
    }

    public override void OpenUIInternal()
    {
        if (_useDim)    // Dimmed UI 사용 시 처리
        {
            DimmedUI dim = (DimmedUI)UIManager.Instance.ShowUI(UIName.UI_Dimmed);
            if (dim != null)
            {
                dim.SetDimmed(this);
            }
        }

        canvasGroup.interactable = false;
        if (_popupElements != null && _popupElements.Length > 0)
        {
            _popupElements.ForEach(e => e.Open(DefaultPopupDuration* _popupDurationRate));

            DOVirtual.DelayedCall(DefaultPopupDuration* _popupDurationRate,
                () => { canvasGroup.interactable = true; })
                .SetUpdate(true);
        }
        else
        {
            canvasGroup.interactable = true;
        }
    }

    public override Tween CloseUIInternal()
    {
        canvasGroup.interactable = false;

        if (_popupElements != null && _popupElements.Length > 0)
        {
            _popupElements?.ForEach(e => e.Close(DefaultPopupDuration * _popupDurationRate));
            return DOVirtual.DelayedCall(DefaultPopupDuration * _popupDurationRate, null).SetUpdate(true);
        }

        // 팝업 애니메이션 요소 없으면 바로 반환
        return null;
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
