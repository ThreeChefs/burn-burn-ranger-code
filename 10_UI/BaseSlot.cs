
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseSlot : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] protected Image iconImg;

    [SerializeField] bool _isCountable = true;
    [ShowIf("_isCountable")]
    [SerializeField] protected TextMeshProUGUI countText;

   
    protected void SetSlot(SlotInfo slotInfo)
    {    
        if(slotInfo.contentSpr != null )
        {
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = slotInfo.contentSpr;
        }

        if(_isCountable && countText != null)
        {
            countText.text = slotInfo.contentCount.ToString();
        }
       
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }
}


public struct SlotInfo
{
    public Sprite contentSpr;
    public bool isCountable;
    public int contentCount;
    public ItemClass contentClassLevel;
}