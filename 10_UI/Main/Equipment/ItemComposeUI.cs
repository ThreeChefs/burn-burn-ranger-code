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
    private ItemInstance[] _materialInstanaces;
    private ItemInstance _resultInstance;
    private int _count;
    private int Count
    {
        get { return _count; }
        set
        {
            _count = Mathf.Min(value, RequiringCount);

            if (_count > 0)
            {
                _allComposeButton.gameObject.SetActive(_count == 0);
                _composeButton.gameObject.SetActive(_count > 0);
            }
        }
    }
    private const int RequiringCount = 3;       // todo: 아이템 등급에 따라 요구 결과 다르게 하기

    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;

        Init();
    }

    private void OnEnable()
    {
        Count = 0;
        _composeButton.onClick.AddListener(OnClickComposeButton);

        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot += OnClickSlotButton;
        }
    }

    private void OnDisable()
    {
        _composeButton.onClick.RemoveAllListeners();

        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot -= OnClickSlotButton;
        }
    }

    protected override void AwakeInternal()
    {
        _materialInstanaces = new ItemInstance[2];
    }

    private void Init()
    {
        UpdateInventoryUI();
    }

    /// <summary>
    /// 합성 버튼 누를 경우 이벤트
    /// </summary>
    private void OnClickComposeButton()
    {
        // 아이템 정보
        _inventory.Remove(_targetInstanace);
        _inventory.Remove(_materialInstanaces[0]);
        _inventory.Remove(_materialInstanaces[1]);
        _inventory.Add(_resultInstance);

        _targetInstanace = null;
        _materialInstanaces[0] = null;
        _materialInstanaces[1] = null;
        _resultInstance = null;

        // 슬롯
        _originSlot.ResetSlot();
        _materialSlots[0].ResetSlot();
        _materialSlots[1].ResetSlot();
        _resultSlot.ResetSlot();
    }

    /// <summary>
    /// 슬롯 누르면 재료 아이템으로 이동
    /// </summary>
    private void OnClickSlotButton(ItemInstance item)
    {
        if (Count == 0)
        {
            AddOriginItem(item);
        }
        else
        {
            AddMaterialItem();
        }
    }

    private void AddOriginItem(ItemInstance item)
    {
        _targetInstanace = item;

        // todo: 다른 아이템 lock 하기
    }

    private void AddMaterialItem()
    {
        Count++;

        if (CheckCompose())
        {
            _resultInstance = new ItemInstance(_targetInstanace.ItemClass + 1, _targetInstanace.ItemData);
            _resultSlot.SetSlot(_resultInstance);
        }
    }

    /// <summary>
    /// 합성할 수 있는지 체크하기
    /// </summary>
    /// <returns></returns>
    private bool CheckCompose()
    {
        return RequiringCount >= Count;
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
