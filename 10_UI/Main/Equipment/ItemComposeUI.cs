using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComposeUI : BaseUI
{
    [SerializeField] private ItemSlot _itemSlotPrefab;
    [SerializeField] private RectTransform _inventoryUI;

    [SerializeField] private ItemSlot _resultSlot;
    [SerializeField] private ItemSlot _originSlot;
    [SerializeField] private ItemSlot[] _materialSlots;

    [SerializeField] private Button _allComposeButton;
    [SerializeField] private Button _composeButton;

    private Inventory _inventory;
    private List<ItemSlot> _inventorySlots = new();

    private ItemInstance _targetInstanace;
    private ItemInstance _resultInstance;
    private int _count;
    private const int RequiringCount = 3;       // todo: 아이템 등급에 따라 요구 결과 다르게 하기

    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;

        Init();
    }

    private void Init()
    {
        UpdateInventoryUI();
    }

    /// <summary>
    /// 슬롯 누르면 재료 아이템으로 이동
    /// </summary>
    private void OnClickSlotButton()
    {

        _count++;
    }

    private void OnClickComposeButton()
    {

    }

    private void AddItemInstance()
    {
        _count = Mathf.Min(_count + 1, RequiringCount);
        if (CheckCompose())
        {
            _resultInstance = new ItemInstance(_targetInstanace.ItemClass + 1, _targetInstanace.ItemData);
            _resultSlot.SetSlot(_resultInstance);
        }
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

        _resultSlot = transform.FindChild<ItemSlot>("Button_ItemSlot_Result");
        _originSlot = transform.FindChild<ItemSlot>("Button_ItemSlot_Origin");
        _materialSlots = new ItemSlot[2];
        _materialSlots[0] = transform.FindChild<ItemSlot>("Button_ItemSlot_Material");
        _materialSlots[1] = transform.FindChild<ItemSlot>("Button_ItemSlot_Material_1");

        _allComposeButton = transform.FindChild<Button>("Button - AllCompose");
        _composeButton = transform.FindChild<Button>("Button - Compose");
    }
#endif
}
