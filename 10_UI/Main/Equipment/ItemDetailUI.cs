using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Outline _itemIconOutline;
    [SerializeField] private TextMeshProUGUI _itemLevel;
    [SerializeField] private TextMeshProUGUI _itemDescription;

    [Header("Skill Info")]
    [SerializeField] private RectTransform _skillInfoParent;
    [SerializeField] List<Image> _skillColors;
    [SerializeField] List<Outline> _skillColorOutlines;
    [SerializeField] List<TextMeshProUGUI> _skillDescriptions;
    private const int MaxSkillCount = 5;
    private const int PrefabSkillInfoHeight = 80;

    [Header("Wallet")]
    [SerializeField] private TextMeshProUGUI _gold;
    [SerializeField] private TextMeshProUGUI _scroll;

    [Header("Buttons")]
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _allLevelUpButton;

    // 캐싱
    private ItemInstance _curItem;
    #endregion

#if UNITY_EDITOR
    private void Reset()
    {
        _itemClassBadge = transform.FindChild<Image>("Image - ClassBadge");
        _itemClassText = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Class");
        _itemName = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Name");
        _itemIcon = transform.FindChild<Image>("Image - ItemIcon");
        _itemIconOutline = transform.FindChild<Outline>("Image - ItemIcon");
        _itemLevel = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Level");
        _itemDescription = transform.FindChild<TextMeshProUGUI>("Text (TMP) - Description");

        _skillInfoParent = transform.FindChild<RectTransform>("SkillList");

        _skillColors = new(MaxSkillCount);
        _skillColorOutlines = new(MaxSkillCount);
        _skillDescriptions = new(MaxSkillCount);
        foreach (Transform child in _skillInfoParent)
        {
            _skillColors.Add(child.FindChild<Image>("Image - Class"));
            _skillColorOutlines.Add(child.FindChild<Outline>("Image - Class"));
            _skillDescriptions.Add(child.FindChild<TextMeshProUGUI>("Text (TMP) - SkillDescription"));
        }

        _gold = transform.FindChild<Transform>("Bar_Gold").FindChild<TextMeshProUGUI>("Text (TMP) - Value");
        _scroll = transform.FindChild<Transform>("Bar_Scroll").FindChild<TextMeshProUGUI>("Text (TMP) - Value");

        _equipButton = transform.FindChild<Button>("Button - Equip");
        _levelUpButton = transform.FindChild<Button>("Button - LevelUp");
        _allLevelUpButton = transform.FindChild<Button>("Button - AllLevelUp");
    }
#endif
}
