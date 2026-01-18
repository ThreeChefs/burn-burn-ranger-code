using System.Collections.Generic;
using UnityEngine;

public class ItemComposeUI : BaseUI
{
    [SerializeField] private ItemSlot _itemSlotPrefab;
    [SerializeField] private RectTransform _inventoryUI;

    private Inventory _inventory;
    private Equipment _equipment;

    List<ItemSlot> _inventorySlots = new();

    private ItemInstance _targetInstanace;
    private int _count;
    private const int RequiringCount = 3;       // todo: 아이템 등급에 따라 요구 결과 다르게 하기

    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;
        _equipment = PlayerManager.Instance.Equipment;

        Init();
    }

    private void Init()
    {
        UpdateInventoryUI();
    }

    private void AddItemInstance()
    {
        if (CheckCompose())
        {

        }
        _count++;
    }

    private bool CheckCompose()
    {
        return RequiringCount >= _count;
    }

    private void UpdateInventoryUI()
    {
        for (int i = _inventorySlots.Count; i < _inventory.Items.Count; i++)
        {
            AddItemSlot(i);
        }

        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            _inventorySlots[i].SetSlot(_inventory.Items[i]);
        }
    }

    private void AddItemSlot(int index)
    {
        ItemSlot itemSlot = Instantiate(_itemSlotPrefab);
        itemSlot.transform.SetParent(_inventoryUI, false);
        _inventorySlots.Add(itemSlot);
        itemSlot.SetSlot(_inventory.Items[index]);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _itemSlotPrefab = AssetLoader.FindAndLoadByName("Button_ItemSlot_Compose").GetComponent<ComposeItemSlot>();
        _inventoryUI = transform.FindChild<RectTransform>("Content");
    }
#endif
}
