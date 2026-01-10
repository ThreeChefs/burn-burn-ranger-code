using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    protected Button _button;


    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick()
    {
        SoundManager.Instance.PlaySfx(SfxName.Sfx_Click);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOScale(1f, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        this.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        this.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);
    }
}
