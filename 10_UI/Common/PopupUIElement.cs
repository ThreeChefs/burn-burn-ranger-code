
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PopupUIElement : MonoBehaviour
{
    [Title("Open")]
    [SerializeField] PopupUIOpenType _openType;
    [SerializeField] Ease _openEase = Ease.OutQuad;

    [Title("Close")]
    [SerializeField] PopupUIOpenType _closeType;
    [SerializeField] Ease _closeEase = Ease.OutQuad;

    [Title("Tween Time")]
    [MinMaxSlider(0f, 1f, true)]
    [SerializeField] Vector2 _tweenTime = Vector2.up;

    RectTransform _parentCanvas;
    RectTransform _rect;
    
    Vector2 _originPos;
    Vector2 _originScale;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _originPos = _rect.anchoredPosition;
        _originScale = _rect.localScale;

        Canvas rootCanvas = _rect.GetComponentInParent<Canvas>();
        _parentCanvas = rootCanvas.transform as RectTransform;
    }

    void CalcTweenTime(float total, Vector2 tween01, out float delay, out float tweenDuration)
    {
        delay = total * tween01.x;
        tweenDuration = total * (tween01.y - tween01.x);
    }

    Vector2 CalcReverseTweenTime()
    {
        return new Vector2(1f - _tweenTime.y, 1f - _tweenTime.x);
    }


    public void Open(float duration)
    {
        if (_parentCanvas == null) return;

        CalcTweenTime(duration, _tweenTime, out float delay, out float tweenDuration);

        Vector2 parentSize = _parentCanvas.rect.size;
        Vector2 selfSize = _rect.rect.size;

        switch (_openType)
        {
            case PopupUIOpenType.Default:
                _rect.localScale = Vector3.zero;
                _rect.DOScale(_originScale, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;

            case PopupUIOpenType.Horizontal:
                _rect.localScale = new Vector3(0, 1, 1);
                _rect.DOScale(_originScale, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;

            case PopupUIOpenType.Vertical:
                _rect.localScale = new Vector3(1, 0, 1);
                _rect.DOScale(_originScale, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveRight:
                _rect.anchoredPosition = _originPos + Vector2.left * _rect.sizeDelta.x;
                _rect.DOAnchorPos(_originPos, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveLeft:
                _rect.anchoredPosition = _originPos + Vector2.right * _rect.sizeDelta.x;
                _rect.DOAnchorPos(_originPos, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveTop:
                _rect.anchoredPosition = _originPos + Vector2.down * _rect.sizeDelta.y;
                _rect.DOAnchorPos(_originPos, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveBottom:
                _rect.anchoredPosition = _originPos + Vector2.up * _rect.sizeDelta.y;
                _rect.DOAnchorPos(_originPos, duration).SetDelay(delay).SetEase(_openEase).SetUpdate(true);
                break;
        }
    }

    public void Close(float duration)
    {
        if (_parentCanvas == null) return;

        Vector2 parentSize = _parentCanvas.rect.size;
        Vector2 selfSize = _rect.rect.size;

        Vector2 reverseTime = CalcReverseTweenTime();
        CalcTweenTime(duration, reverseTime, out float delay, out float tweenDuration);

        switch (_closeType)
        {
            case PopupUIOpenType.Default:
                _rect.DOScale(0f, tweenDuration).SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;

            case PopupUIOpenType.Horizontal:
                _rect.DOScale(new Vector3(0, 1, 1), tweenDuration).SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;

            case PopupUIOpenType.Vertical:
                _rect.DOScale(new Vector3(1, 0, 1), tweenDuration).SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveRight:
                _rect.DOAnchorPos(_rect.anchoredPosition + Vector2.right * _rect.sizeDelta.x, tweenDuration)
                    .SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveLeft:
                _rect.DOAnchorPos(_rect.anchoredPosition + Vector2.left * _rect.sizeDelta.x, tweenDuration)
                    .SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveTop:
                _rect.DOAnchorPos(_rect.anchoredPosition + Vector2.up * _rect.sizeDelta.y, tweenDuration)
                    .SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;

            case PopupUIOpenType.MoveBottom:
                _rect.DOAnchorPos(_rect.anchoredPosition + Vector2.down * _rect.sizeDelta.y, tweenDuration)
                    .SetDelay(delay).SetEase(_closeEase).SetUpdate(true);
                break;
        }
    }
}



