
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

    RectTransform _rect;
    Vector2 _originPos;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _originPos = _rect.anchoredPosition;
    }

    public void Open(float _duration)
    {
        switch (_openType)
        {

            case PopupUIOpenType.Default:
                if (_rect != null)
                {
                    _rect.localScale = Vector3.zero;
                    _rect.DOScale(1f, _duration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Horizontal:
                if (_rect != null)
                {
                    _rect.localScale = new Vector3(0, 1, 1);
                    _rect.DOScale(1f, _duration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Vertical:
                if (_rect != null)
                {
                    _rect.localScale = new Vector3(1, 0, 1);
                    _rect.DOScale(1f, _duration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;
        }
    }

    public void Close(float _duration)
    {
        switch (_closeType)
        {
            case PopupUIOpenType.Default:
                if (_rect != null)
                {
                    _rect.DOScale(0f, _duration).SetEase(Ease.InQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Horizontal:
                if (_rect != null)
                {
                    _rect.DOScale(new Vector3(0, 1, 1), _duration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;

            case PopupUIOpenType.Vertical:
                if (_rect != null)
                {
                    _rect.DOScale(new Vector3(1, 0, 1), _duration).SetEase(Ease.OutQuad).SetUpdate(true);
                }
                break;
        }
    }

}
