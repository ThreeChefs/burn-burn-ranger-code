using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRewardListPanel : MonoBehaviour
{
    [SerializeField] ItemSlot _itemSlotOrigin;
    [SerializeField] LayoutGroup _layoutGroup;


    public void SetRewardList(List<ItemInstance> itemInstance)
    {
        for (int i = 0; i < itemInstance.Count; i++)
        {
            ItemSlot newSlot = Instantiate(_itemSlotOrigin, _layoutGroup.transform);
            newSlot.SetSlot(itemInstance[i]);
        }
    }


}
