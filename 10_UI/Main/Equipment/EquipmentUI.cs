using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : BaseUI
{
    // todo: json으로 빼거나 default인 거 체크하기
    [SerializeField] private List<ItemData> _defaultData;
    [SerializeField] private ItemSlot _itemSlotPrefab;

    private Inventory _inventory;

    // 장착 중인 장비
    [SerializeField] private Transform _equipmentSlotParent;
    private Dictionary<EquipmentType, ItemSlot> _equipmentSlots;

    // 장비 인벤토리 
    [SerializeField] private RectTransform _inventoryUI;
    private List<ItemSlot> _inventorySlots = new();

    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;
        Init();

        _inventory.OnInventoryChanged += UpdateInventoryUI;
    }

    protected override void AwakeInternal()
    {
        _equipmentSlots = new();
        foreach (Transform child in _equipmentSlotParent)
        {
            string[] tokens = child.name.Split("Button_ItemSlot_");
            if (tokens.Length > 1 && Enum.TryParse(tokens[1], out EquipmentType type))
            {
                _equipmentSlots[type] = child.GetComponent<ItemSlot>();
            }
        }
    }

    private void Init()
    {
        // todo: 초기 데이터 나중에 어떻게 할지 얘기해보긴 해야함
        _defaultData.ForEach(data => _inventory.Add(new ItemInstance(ItemClass.Normal, data)));

        // todo: 장비 슬롯도 초기화
        foreach (ItemInstance item in _inventory.Items)
        {
            ItemSlot itemSlot = Instantiate(_itemSlotPrefab);
            _inventorySlots.Add(itemSlot);
            itemSlot.SetSlot(item);
            itemSlot.transform.SetParent(_inventoryUI, false);
        }

        //_inventorySlots.Sort();
    }

    /// <summary>
    /// 인벤토리에 들어온 마지막 아이템 슬롯 만들어서 적용
    /// </summary>
    private void UpdateInventoryUI()
    {
        ItemSlot itemSlot = Instantiate(_itemSlotPrefab);
        _inventorySlots.Add(itemSlot);
        itemSlot.SetSlot(_inventory.Items[_inventory.Items.Count - 1]);
        itemSlot.transform.SetParent(_inventoryUI, false);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _defaultData.Add(AssetLoader.FindAndLoadByName<ItemData>("Kunai"));
        _defaultData.Add(AssetLoader.FindAndLoadByName<ItemData>("MilitaryUniform"));
        _itemSlotPrefab = AssetLoader.FindAndLoadByName("ItemSlot").GetComponent<ItemSlot>();
        _equipmentSlotParent = transform.FindChild<Transform>("Slots");
        _inventoryUI = transform.FindChild<RectTransform>("Content");
    }
#endif
}
