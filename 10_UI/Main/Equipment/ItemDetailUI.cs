using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// UI - 아이템 상세 정보 창
/// </summary>
public class ItemDetailUI : BaseUI
{
    #region 필드
    // 컴포넌트
    [Header("Header")]
    [SerializeField] private Image _itemClassBadge;
    [SerializeField] private TextMeshProUGUI _itemClassText;
    [SerializeField] private TextMeshProUGUI _itemName;

    [Header("Item Info")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _statIcon;
    [SerializeField] private TextMeshProUGUI _statValue;
    [SerializeField] private TextMeshProUGUI _itemLevel;
    [SerializeField] private TextMeshProUGUI _itemDescription;
    [SerializeField] private Sprite _attackIcon;
    [SerializeField] private Sprite _healthIcon;
    private Outline _itemIconOutline;

    [Header("Skill Info")]
    [SerializeField] private RectTransform _skillInfoParent;
    [SerializeField] private GameObject[] _skillDetails;
    private Image[] _skillColors;
    private Outline[] _skillColorOutlines;
    private TextMeshProUGUI[] _skillDescriptions;
    private const int MaxSkillCount = 5;

    [Header("Wallet")]
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _scrollText;
    private Wallet _gold;
    private Wallet _scroll;

    [Header("Buttons")]
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _allLevelUpButton;
    private TextMeshProUGUI _equipButtonText;

    // 캐싱
    private ItemInstance _curItem;
    #endregion

    #region Unity API
    private void Start()
    {
        _gold = PlayerManager.Instance.Wallet[WalletType.Gold];
        // todo: 스크롤으로 변경
        _scroll = PlayerManager.Instance.Wallet[WalletType.Gem];
    }

    private void OnEnable()
    {
        //버튼
        _equipButton.onClick.AddListener(Equip);

        // 아이템 이벤트 구독
    }

    private void OnDisable()
    {
        _equipButton.onClick.RemoveAllListeners();

        // 아이템 이벤트 구독 해제
    }
    #endregion

    #region 초기화
    protected override void AwakeInternal()
    {
        base.AwakeInternal();

        _itemIconOutline = _itemIcon.GetComponent<Outline>();
        _equipButtonText = _equipButton.GetComponentInChildren<TextMeshProUGUI>(true);

        ResetList();
    }

    private void ResetList()
    {
        _skillColors = new Image[MaxSkillCount];
        _skillColorOutlines = new Outline[MaxSkillCount];
        _skillDescriptions = new TextMeshProUGUI[MaxSkillCount];

        for (int i = 0; i < MaxSkillCount; i++)
        {
            _skillColors[i] = _skillDetails[i].GetComponentInChildren<Image>();
            _skillColorOutlines[i] = _skillDetails[i].GetComponentInChildren<Outline>();
            _skillDescriptions[i] = _skillDetails[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    #endregion

    public void SetItem(ItemInstance instance)
    {
        if (_curItem != null && _curItem.Equals(instance)) return;
        _curItem = instance;

        ItemClass itemClass = instance.ItemClass;
        ItemData itemData = instance.ItemData;

        _itemClassBadge.color = ItemUtils.GetClassColor(itemClass);
        _itemClassText.text = ItemUtils.GetClassString(itemClass);
        _itemName.text = itemData.DisplayName;

        _itemIcon.sprite = itemData.Icon;
        _itemIconOutline.effectColor = ItemUtils.GetHighlightColor(itemClass);
        _itemLevel.text = $"레벨: {instance.Level}/{ItemUtils.GetClassMaxLevel(itemClass)}";
        _statIcon.sprite = itemData.EquipmentType == EquipmentType.Weapon ? _attackIcon : _healthIcon;
        _statValue.text = "100";       // todo: item utils에서 스탯 계산식 구현 후 반영
        _itemDescription.text = itemData.Description;

        for (int i = 0; i < MaxSkillCount; i++)
        {
            if (i < itemData.Equipments.Length)
            {
                // todo: item class로 lock/unlock 표기하기
                _skillColors[i].color = ItemUtils.GetClassColor(itemClass);
                _skillColorOutlines[i].effectColor = ItemUtils.GetHighlightColor(itemClass);
                _skillDescriptions[i].text = itemData.Equipments[i].Description;
                _skillDescriptions[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                _skillColors[i].gameObject.SetActive(false);
                _skillDescriptions[i].transform.parent.gameObject.SetActive(false);
            }
        }

        _goldText.text = $"{_gold.Value}/요구골드";
        _scrollText.text = $"{_scroll.Value}/요구스크롤";

        _equipButtonText.text = instance.IsEquipped ? "장착 해제" : "장착";
    }

    private void Equip()
    {
        PlayerManager.Instance.Equipment.Equip(_curItem);
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _itemClassBadge = transform.FindChild<Image>("Image - ClassBadge");
        _itemClassText = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Class");
        _itemName = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Name");

        _itemIcon = transform.FindChild<Image>("Image - ItemIcon");
        _statIcon = transform.FindChild<Image>("Image - StatIcon");
        _statValue = transform.FindChild<TextMeshProUGUI>("Text (TMP) - StatValue");
        _itemLevel = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Level");
        _itemDescription = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Description");

        _attackIcon = LoadIcon256("ItemIcon_Gear_Sword");
        _healthIcon = LoadIcon256("ItemIcon_Heart_Red");

        _skillInfoParent = transform.FindChild<RectTransform>("SkillList");
        _skillDetails = new GameObject[MaxSkillCount];
        for (int i = 0; i < MaxSkillCount; i++)
        {
            _skillDetails[i] = _skillInfoParent.GetChild(i).gameObject;
        }

        _goldText = transform.FindChild<Transform>("Bar_Gold").FindChild<TextMeshProUGUI>("Text (TMP) - Value");
        _scrollText = transform.FindChild<Transform>("Bar_Scroll").FindChild<TextMeshProUGUI>("Text (TMP) - Value");

        _equipButton = transform.FindChild<Button>("Button - Equip");
        _levelUpButton = transform.FindChild<Button>("Button - LevelUp");
        _allLevelUpButton = transform.FindChild<Button>("Button - AllLevelUp");
    }

    private const string ICON_256_PATH = "Assets/10_Artworks/99_External/Layer Lab/GUI Pro-SuperCasual/ResourcesData/Sprites/Components/Icon_ItemIcons/256/";

    private Sprite LoadIcon256(string fileName)
    {
        return AssetDatabase.LoadAssetAtPath<Sprite>(ICON_256_PATH + fileName + ".png");
    }
#endif
}
