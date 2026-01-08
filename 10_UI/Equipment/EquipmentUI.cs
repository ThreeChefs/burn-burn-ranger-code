using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : BaseUI
{
    // todo: json으로 빼거나 default인 거 체크하기
    [SerializeField] private List<ItemData> _defaultData;
    [SerializeField] private ItemSlot _itemSlotPrefab;

    private Inventory _inventory;

    // 장착 중인 장비
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

    private void OnDestroy()
    {
        _inventory.OnInventoryChanged -= UpdateInventoryUI;
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
        _inventoryUI = transform.FindChild<RectTransform>("Content");
    }
#endif
}
