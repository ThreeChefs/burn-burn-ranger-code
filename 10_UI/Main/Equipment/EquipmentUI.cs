using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : BaseUI
{
    [SerializeField] private ItemSlot _itemSlotPrefab;

    private Inventory _inventory;
    private Equipment _equipment;

    // 장착 중인 장비
    [SerializeField] private Transform _equipmentSlotParent;
    private Dictionary<EquipmentType, ItemSlot> _equipmentSlots;
    private int _equipmentCount;

    // 장비 인벤토리 
    [SerializeField] private RectTransform _inventoryUI;
    private List<ItemSlot> _inventorySlots = new();

    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;
        _equipment = PlayerManager.Instance.Equipment;
        Init();

        _inventory.OnInventoryChanged += UpdateInventoryUI;
        _equipment.OnEquipmentChanged += UpdateEquipUI;
    }

    private void OnEnable()
    {
        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= UpdateInventoryUI;
            _inventory.OnInventoryChanged += UpdateInventoryUI;
        }
        if (_equipment != null)
        {
            _equipment.OnEquipmentChanged -= UpdateEquipUI;
            _equipment.OnEquipmentChanged += UpdateEquipUI;
        }
    }

    private void OnDisable()
    {
        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= UpdateInventoryUI;
        }
        if (_equipment != null)
        {
            _equipment.OnEquipmentChanged -= UpdateEquipUI;
        }
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
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            AddItemSlot();
        }

        UpdateEquipUI();

        // todo: 레이아웃 대신 직접 계산
        //_inventoryUI.GetComponent<GridLayoutGroup>().enabled = false;
        //_inventoryUI.GetComponent<ContentSizeFitter>().enabled = false;
    }

    private void AddItemSlot()
    {
        ItemSlot itemSlot = Instantiate(_itemSlotPrefab);
        itemSlot.transform.SetParent(_inventoryUI, false);
        _inventorySlots.Add(itemSlot);
    }

    /// <summary>
    /// 인벤토리 전부 순회하며 슬롯 업데이트 하기
    /// todo: 리스트 전부 순회 말고 단순화 방법 있나 생각해보기
    /// 앞의 슬롯이 빠지면 다시 그려줘야한다는 건 변함이 없기는 함
    /// </summary>
    private void UpdateEquipUI()
    {
        ItemInstance item = null;
        int itemIndex = 0;

        // todo: 인벤토리 정렬

        for (int slotIndex = 0; slotIndex < _inventorySlots.Count; slotIndex++)
        {
            // 장착한 장비일 경우 스킵
            if (!TryGetNextUnequippedItem(ref itemIndex, out item)) break;

            _inventorySlots[slotIndex].SetSlot(item);
            _inventorySlots[slotIndex].gameObject.SetActive(true);
            Logger.Log($"인벤토리 {slotIndex} 번째 장비 슬롯에 설정");
        }

        // 남은 슬롯 비활성화
        for (int j = itemIndex - _equipmentCount; j < _inventorySlots.Count; j++)
        {
            _inventorySlots[j].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 장착한 장비인지 확인하고, 아닐 경우 아이템 반환
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool TryGetNextUnequippedItem(ref int index, out ItemInstance item)
    {
        while (index < _inventory.Items.Count)
        {
            item = _inventory.Items[index];
            index++;

            // 장착한 장비 시 스킵
            if (_equipment.IsEquip(item))
            {
                UpdateEquipmentSlot(item, EquipmentApplyType.Equip);
                continue;
            }

            return true;
        }

        item = null;
        return false;
    }

    private void UpdateEquipmentSlot(ItemInstance item, EquipmentApplyType applyType)
    {
        ItemSlot equipmentSlot = _equipmentSlots[item.ItemData.EquipmentType];
        if (applyType == EquipmentApplyType.Equip)
        {
            equipmentSlot.SetSlot(item);
            _equipmentCount = Math.Min(_equipmentCount + 1, _equipmentSlots.Count);
            Logger.Log($"{item.ItemData.EquipmentType} 타입 장비 장착");
        }
        else
        {
            equipmentSlot.ResetSlot();
            _equipmentCount = Math.Max(_equipmentCount - 1, 0);
        }
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
        _itemSlotPrefab = AssetLoader.FindAndLoadByName("ItemSlot").GetComponent<ItemSlot>();
        _equipmentSlotParent = transform.FindChild<Transform>("Slots");
        _inventoryUI = transform.FindChild<RectTransform>("Content");
    }
#endif
}
