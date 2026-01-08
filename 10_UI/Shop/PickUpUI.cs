using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 뽑기 후 획득 UI
/// </summary>
public class PickUpUI : BaseUI
{
    [SerializeField] ItemSlot[] slots;

    private void OnDisable()
    {
        foreach (ItemSlot slot in slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void PickUpItems(List<ItemInstance> instances)
    {
        for (int i = 0; i < instances.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].SetSlot(instances[i]);
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        slots = GetComponentsInChildren<ItemSlot>();
    }
#endif
}
