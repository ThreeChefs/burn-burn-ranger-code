using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : BaseUI
{
    // todo: json으로 빼거나 default인 거 체크하기
    [SerializeField] private List<ItemData> _defaultData;
    [SerializeField] private ItemSlot _itemSlotPrefab;

    private Inventory _inventory;
    private Equipment _equipment;

    // 장착 중인 장비
    [SerializeField] private Transform _equipmentSlotParent;
    private Dictionary<EquipmentType, ItemSlot> _equipmentSlots;

    // 장비 인벤토리 
    [SerializeField] private RectTransform _inventoryUI;
    private List<ItemSlot> _inventorySlots = new();

    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;
        _equipment = PlayerManager.Instance.Equipment;
        Init();

        _inventory.OnInventoryChanged += UpdateInventoryUI;
        _equipment.OnEquipmentChanged += HandleUpdateEquipmentSlot;
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
            _equipment.OnEquipmentChanged -= HandleUpdateEquipmentSlot;
            _equipment.OnEquipmentChanged += HandleUpdateEquipmentSlot;
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
            _equipment.OnEquipmentChanged -= HandleUpdateEquipmentSlot;
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
        // todo: 초기 데이터 나중에 어떻게 할지 얘기해보긴 해야함
        _defaultData.ForEach(data => _inventory.Add(new ItemInstance(ItemClass.Normal, data)));

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
        int i = 0;
        for (int j = 0; i < _inventory.Items.Count && j < _inventorySlots.Count; i++, j++)
        {
            ItemInstance item = _inventory.Items[i];

            // 장착한 장비 시 스킵
            if (_equipment.IsEquip(item))
            {
                _equipmentSlots[item.ItemData.EquipmentType].SetSlot(item);
                i++;
                continue;
            }

            _inventorySlots[j].SetSlot(item);
            _inventorySlots[j].gameObject.SetActive(true);
        }

        for (int j = i; j < _inventorySlots.Count; j++)
        {
            _inventorySlots[j].gameObject.SetActive(false);
        }
        //_inventorySlots.Sort();
    }

    private void HandleUpdateEquipmentSlot(ItemInstance item, EquipmentApplyType applyType)
    {
        ItemSlot equipmentSlot = _equipmentSlots[item.ItemData.EquipmentType];
        if (applyType == EquipmentApplyType.Equip)
        {
            equipmentSlot.SetSlot(item);
        }
        else
        {
            equipmentSlot.ResetSlot();
        }

        UpdateEquipUI();
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
