using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComposeUI : BaseUI
{
    [SerializeField] private ComposeItemSlot _itemSlotPrefab;
    [SerializeField] private RectTransform _inventoryUI;

    [SerializeField] private ItemSlot _resultSlot;
    [SerializeField] private MaterialItemSlot[] _materialSlots;

    [SerializeField] private Button _allComposeButton;
    [SerializeField] private Button _composeButton;

    private Inventory _inventory;
    private List<ComposeItemSlot> _inventorySlots = new();

    private ItemInstance[] _materialInstanaces;             // 실제 재료 아이템
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

    #region Unity API
    private void Start()
    {
        _inventory = PlayerManager.Instance.Inventory;
        _inventory.OnInventoryChanged += UpdateInventoryUI;

        Init();
    }

    private void OnEnable()
    {
        // 합성 버튼
        _composeButton.onClick.AddListener(OnClickComposeButton);

        // 인벤토리 슬롯
        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot += OnClickInventorySlotButton;
        }

        // 합성 재료 슬롯
        _materialSlots[0].OnClickSlot += OnClickOriginMaterialButton;

        ResetMaterialSlots();

        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= UpdateInventoryUI;
            _inventory.OnInventoryChanged += UpdateInventoryUI;
            UpdateInventoryUI();
        }
    }

    private void OnDisable()
    {
        // 합성 버튼
        _composeButton.onClick.RemoveAllListeners();

        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= UpdateInventoryUI;
        }

        // 인벤토리 슬롯
        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot -= OnClickInventorySlotButton;
        }
    }
    #endregion

    #region 초기화
    protected override void AwakeInternal()
    {
        _materialInstanaces = new ItemInstance[RequiringCount];
    }

    private void Init()
    {
        UpdateInventoryUI();

        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot += OnClickInventorySlotButton;
        }
    }

    private void ResetMaterialSlots()
    {
        Count = 0;

        _resultSlot.ResetSlot();
        for (int i = 0; i < _materialSlots.Length; i++)
        {
            _materialSlots[i].ResetSlot();
        }

        for (int i = 0; i < _materialInstanaces.Length; i++)
        {
            _materialInstanaces[i] = null;
        }
        _resultInstance = null;

        foreach (ComposeItemSlot slot in _inventorySlots)
        {
            slot.UnLockButton();
        }
    }
    #endregion

    #region 버튼 - 합성
    /// <summary>
    /// 합성 버튼 누를 경우 이벤트
    /// </summary>
    private void OnClickComposeButton()
    {
        if (!CheckCompose()) return;

        // 아이템 삭제 & 주기
        for (int i = 0; i < _materialInstanaces.Length; i++)
        {
            if (PlayerManager.Instance.Equipment.IsEquip(_materialInstanaces[i]))
            {
                PlayerManager.Instance.Equipment.Unequip(_materialInstanaces[i]);
            }
            _inventory.Remove(_materialInstanaces[i]);
        }
        _inventory.Add(_resultInstance);

        ResetMaterialSlots();   // 슬롯 정보 리셋
        UpdateInventoryUI();    // 인벤토리 ui 리셋
    }
    #endregion

    #region 버튼 - 인벤토리 슬롯
    /// <summary>
    /// 인벤토리 내부 슬롯 누르면 재료 아이템으로 이동
    /// </summary>
    private void OnClickInventorySlotButton(ComposeItemSlot slot, ItemInstance item)
    {
        if (Count < RequiringCount)
        {
            AddMaterialItem(slot, item);
        }
        UpdateInventoryUI();
    }

    /// <summary>
    /// 인벤토리에 있는 아이템 재표 아이템에 넣기
    /// </summary>
    /// <param name="item"></param>
    private void AddMaterialItem(ComposeItemSlot slot, ItemInstance item)
    {
        _materialInstanaces[Count] = item;
        _materialSlots[Count].SetSlot(_materialInstanaces[Count], slot);

        if (Count == 0)     // 아이템을 처음 고를 때만 
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].EqualsItemClassAndData(_materialInstanaces[0])) continue;
                _inventorySlots[i].LockButton();
            }
        }

        Count++;

        if (CheckCompose())
        {
            _resultInstance = new ItemInstance(_materialInstanaces[0].ItemClass + 1, _materialInstanaces[0].ItemData);
            _resultSlot.SetSlot(_resultInstance);
        }
    }

    /// <summary>
    /// 합성할 수 있는지 체크하기
    /// </summary>
    /// <returns></returns>
    private bool CheckCompose()
    {
        // 재료 아이템의 개수가 필요한 개수 이상일 때 && 재료 아이템 다음 등급이 레전드리 이하일 때
        return RequiringCount <= Count
            && _materialInstanaces[0].ItemClass + 1 <= ItemClass.Legendary;
    }
    #endregion

    #region 버튼 - 재료 슬롯
    /// <summary>
    /// 원본 재료 슬롯 눌렀을 때 이벤트
    /// </summary>
    private void OnClickOriginMaterialButton()
    {
        ResetMaterialSlots();
        UpdateInventoryUI();
    }
    #endregion

    private void UpdateInventoryUI()
    {
        // 아이템 슬롯이 인벤토리 슬롯보다 적을 경우
        for (int i = _inventorySlots.Count; i < _inventory.Items.Count; i++)
        {
            AddItemSlot(i);
        }

        // 아이템 슬롯에 인벤토리 아이템 적용
        ItemInstance item = null;
        int itemIndex = 0;
        int slotIndex = 0;
        while (slotIndex < _inventorySlots.Count
            && TryGetNextMaterialItem(ref itemIndex, out item))
        {
            _inventorySlots[slotIndex].SetSlot(item);
            _inventorySlots[slotIndex].gameObject.SetActive(true);
            slotIndex++;
        }

        // 아이템 슬롯이 인벤토리 슬롯보다 많을 경우
        for (int i = slotIndex; i < _inventorySlots.Count; i++)
        {
            _inventorySlots[i].gameObject.SetActive(false);
        }

        // 재료 아이템에 연결되어 있는 슬롯일 경우 비활성화
        foreach (MaterialItemSlot slot in _materialSlots)
        {
            slot.Target?.gameObject.SetActive(false);
        }
    }

    private bool TryGetNextMaterialItem(ref int index, out ItemInstance item)
    {
        while (index < _inventory.Items.Count)
        {
            item = _inventory.Items[index];
            index++;

            if (item.ItemClass >= ItemClass.Legendary) continue;

            return true;
        }

        item = null;
        return false;
    }

    private void AddItemSlot(int index)
    {
        ComposeItemSlot itemSlot = Instantiate(_itemSlotPrefab);
        itemSlot.transform.SetParent(_inventoryUI, false);
        _inventorySlots.Add(itemSlot);
        itemSlot.SetSlot(_inventory.Items[index]);
        itemSlot.OnClickSlot += OnClickInventorySlotButton;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _itemSlotPrefab = AssetLoader.FindAndLoadByName("Button_ItemSlot_Compose").GetComponent<ComposeItemSlot>();
        _inventoryUI = transform.FindChild<RectTransform>("Content");

        _resultSlot = transform.FindChild<ItemSlot>("Button_ItemSlot_Result");
        _materialSlots = new MaterialItemSlot[3];
        _materialSlots[0] = transform.FindChild<MaterialItemSlot>("Button_ItemSlot_Material");
        _materialSlots[1] = transform.FindChild<MaterialItemSlot>("Button_ItemSlot_Material_1");
        _materialSlots[2] = transform.FindChild<MaterialItemSlot>("Button_ItemSlot_Material_2");

        _allComposeButton = transform.FindChild<Button>("Button - AllCompose");
        _composeButton = transform.FindChild<Button>("Button - Compose");
    }
#endif
}
