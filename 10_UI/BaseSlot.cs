
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlot : MonoBehaviour
{
    [SerializeField] protected Image iconImg;
    [SerializeField] protected Image itemClass;
    [SerializeField] protected TextMeshProUGUI countText;

    public virtual void SetSlot(SlotInfo slotInfo)
    {    
        if(slotInfo.ContentImg != null )
        {
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = slotInfo.ContentImg;
        }

        countText.text = slotInfo.ContentCount.ToString();

        if(slotInfo.ContentClassLevel != ItemClass.None)
        {
            itemClass.gameObject.SetActive(true);
            itemClass.color = ItemClassColor.GetClassColor(slotInfo.ContentClassLevel);
        }
        else
        {
            itemClass.gameObject.SetActive(false);
        }

    }

}


public struct SlotInfo
{
    public Sprite ContentImg;
    public int ContentCount;
    public ItemClass ContentClassLevel;
}