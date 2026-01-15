using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRewardListPanel : MonoBehaviour
{
    [SerializeField] ItemSlot _itemSlotOrigin;
    [SerializeField] UpgradeMaterialSlot _upgradeMaterialSlotOrigin;
    [SerializeField] LayoutGroup _layoutGroup;


    public void SetRewardList(List<StageRewardInfo> rewards)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards != null)
            {
                if (rewards[i].type == ItemType.Equipment)
                {
                    ItemSlot newItemSlot = Instantiate(_itemSlotOrigin, _layoutGroup.transform);
                    newItemSlot.SetSlot(rewards[i].itemInfo);
                }
                else if (rewards[i].type == ItemType.UpgradeMaterial)
                {
                    // 재료 슬롯 채우기
                    UpgradeMaterialSlot newSlot = Instantiate(_upgradeMaterialSlotOrigin, _layoutGroup.transform);
                    newSlot.SetSlot(rewards[i].upgradeMaterialType, rewards[i].count);
                }
            }
        }
    }


}
