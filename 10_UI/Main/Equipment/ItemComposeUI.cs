using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComposeUI : BaseUI
{
    [SerializeField] private ComposeItemSlot _itemSlotPrefab;
    [SerializeField] private RectTransform _inventoryUI;

    [SerializeField] private ComposeItemSlot _resultSlot;
    [SerializeField] private ComposeItemSlot[] _materialSlots;

    [SerializeField] private Button _allComposeButton;
    [SerializeField] private Button _composeButton;

    private Inventory _inventory;
    private List<ComposeItemSlot> _inventorySlots = new();

    private ItemInstance[] _materialInstanaces;             // 실제 재료 아이템
    private ComposeItemSlot[] _materialTargetSlots;         // 재료 아이템 슬롯 - 재료 뺄 때 바로 빼기 위해 캐싱
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
            item.OnClickSlot += OnClickInventorySlotButton;
        }
    }

    private void OnDisable()
    {
        _composeButton.onClick.RemoveAllListeners();

        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot -= OnClickInventorySlotButton;
        }
    }

    protected override void AwakeInternal()
    {
        _materialInstanaces = new ItemInstance[RequiringCount];
        _materialTargetSlots = new ComposeItemSlot[RequiringCount];
    }

    private void Init()
    {
        UpdateInventoryUI();

        foreach (ComposeItemSlot item in _inventorySlots)
        {
            item.OnClickSlot += OnClickInventorySlotButton;
        }
    }

    /// <summary>
    /// 합성 버튼 누를 경우 이벤트
    /// </summary>
    private void OnClickComposeButton()
    {
        // 아이템 정보
        for (int i = 0; i < _materialInstanaces.Length; i++)
        {
            _inventory.Remove(_materialInstanaces[i]);
            _materialInstanaces[i] = null;
        }
        _inventory.Add(_resultInstance);
        _resultInstance = null;

        // 슬롯
        foreach (ComposeItemSlot slot in _materialSlots)
        {
            slot.ResetSlot();
        }
        _resultSlot.ResetSlot();
    }

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
        if (Count == 0)     // 아이템을 처음 고를 때만 
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].EqualsItemClassAndData(_materialInstanaces[0])) continue;
                _inventorySlots[i].LockButton();
            }
        }

        _materialTargetSlots[Count] = slot;
        _materialInstanaces[Count] = item;
        _materialSlots[Count].SetSlot(_materialInstanaces[Count]);
        slot.IsMaterial = true;

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
            if (_inventorySlots[i].IsMaterial) continue;
            _inventorySlots[i].SetSlot(_inventory.Items[i]);
        }
    }

    private void AddItemSlot(int index)
    {
        ComposeItemSlot itemSlot = Instantiate(_itemSlotPrefab);
        itemSlot.transform.SetParent(_inventoryUI, false);
        _inventorySlots.Add(itemSlot);
        itemSlot.SetSlot(_inventory.Items[index]);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _itemSlotPrefab = AssetLoader.FindAndLoadByName("Button_ItemSlot_Compose").GetComponent<ComposeItemSlot>();
        _inventoryUI = transform.FindChild<RectTransform>("Content");

        _resultSlot = transform.FindChild<ComposeItemSlot>("Button_ItemSlot_Result");
        _materialSlots = new ComposeItemSlot[3];
        _materialSlots[0] = transform.FindChild<ComposeItemSlot>("Button_ItemSlot_Material");
        _materialSlots[1] = transform.FindChild<ComposeItemSlot>("Button_ItemSlot_Material_1");
        _materialSlots[2] = transform.FindChild<ComposeItemSlot>("Button_ItemSlot_Material_2");

        _allComposeButton = transform.FindChild<Button>("Button - AllCompose");
        _composeButton = transform.FindChild<Button>("Button - Compose");
    }
#endif
}
