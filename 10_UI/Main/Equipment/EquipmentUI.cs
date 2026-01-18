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

    // 장비 인벤토리 
    [SerializeField] private RectTransform _inventoryUI;
    private List<ItemSlot> _inventorySlots = new();

    #region Unity API
    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;
        _equipment = PlayerManager.Instance.Equipment;
        Init();

        _equipment.OnEquipmentChanged += UpdateEquipUI;
    }

    private void OnEnable()
    {
        if (_inventory != null)
        {
            UpdateInventoryUI();
        }

        if (_equipment != null)
        {
            _equipment.OnEquipmentChanged -= UpdateEquipUI;
            _equipment.OnEquipmentChanged += UpdateEquipUI;
        }
    }

    private void OnDisable()
    {
        if (_equipment != null)
        {
            _equipment.OnEquipmentChanged -= UpdateEquipUI;
        }
    }
    #endregion

    #region 초기화
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
        UpdateInventoryUI();
        UpdateEquipUI();

        // todo: 레이아웃 대신 직접 계산
        //_inventoryUI.GetComponent<GridLayoutGroup>().enabled = false;
        //_inventoryUI.GetComponent<ContentSizeFitter>().enabled = false;
    }
    #endregion

    #region UI 업데이트
    /// <summary>
    /// 인벤토리 전부 순회하며 슬롯 업데이트 하기
    /// todo: 리스트 전부 순회 말고 단순화 방법 있나 생각해보기
    /// 앞의 슬롯이 빠지면 다시 그려줘야한다는 건 변함이 없기는 함
    /// </summary>
    private void UpdateEquipUI()
    {
        UpdateEquipmentSlots();

        ItemInstance item = null;
        int itemIndex = 0;
        int slotIndex = 0;

        // todo: 인벤토리 정렬

        while (slotIndex < _inventorySlots.Count
            && TryGetNextUnequippedItem(ref itemIndex, out item))
        {
            _inventorySlots[slotIndex].SetSlot(item);
            _inventorySlots[slotIndex].gameObject.SetActive(true);
            //Logger.Log($"인벤토리 {slotIndex} 번째 장비 슬롯에 설정");
            slotIndex++;
        }

        // 남은 슬롯 비활성화
        for (int i = slotIndex; i < _inventorySlots.Count; i++)
        {
            _inventorySlots[i].gameObject.SetActive(false);
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
            if (_equipment.IsEquip(item)) continue;

            return true;
        }

        item = null;
        return false;
    }

    private void UpdateEquipmentSlots()
    {
        // 리셋
        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            _equipmentSlots[type].ResetSlot();
        }

        ItemSlot slot;
        foreach (KeyValuePair<EquipmentType, ItemInstance> pair in _equipment.Equipments)
        {
            if (pair.Value == null)
            {
                slot = _equipmentSlots[pair.Key];
                slot.ResetSlot();
            }
            else
            {
                slot = _equipmentSlots[pair.Key];
                slot.SetSlot(pair.Value);
                Logger.Log($"{pair.Value.ItemData.EquipmentType} 타입 장비 장착");
            }
        }
    }

    /// <summary>
    /// 인벤토리 슬롯 만들기
    /// </summary>
    private void UpdateInventoryUI()
    {
        for (int i = _inventorySlots.Count; i < _inventory.Items.Count; i++)
        {
            AddItemSlot(i);
        }
    }

    private void AddItemSlot(int index)
    {
        ItemSlot itemSlot = Instantiate(_itemSlotPrefab);
        itemSlot.transform.SetParent(_inventoryUI, false);
        _inventorySlots.Add(itemSlot);
        itemSlot.SetSlot(_inventory.Items[index]);
    }
    #endregion

#if UNITY_EDITOR
    private void Reset()
    {
        _itemSlotPrefab = AssetLoader.FindAndLoadByName("Button_ItemSlot_Inventory").GetComponent<InventoryItemSlot>();
        _equipmentSlotParent = transform.FindChild<Transform>("EquipmentSlotList");
        _inventoryUI = transform.FindChild<RectTransform>("Content");
    }
#endif
}
