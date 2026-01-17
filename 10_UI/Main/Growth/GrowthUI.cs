using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthUI : BaseUI
{
    [Title("UI")]
    [SerializeField] RectTransform _content;
    [SerializeField] VerticalLayoutGroup _layoutGroup;
    [SerializeField] RectTransform _spacing;
    [SerializeField] GrowthBubblePanel _panel;
    [SerializeField] Button _backButton;

    [Title("프리팹")]
    [SerializeField] GrowthSlot _slotOrigin;

    [Title("Sprite")]
    [SerializeField] Sprite _attackSpr;
    [SerializeField] Sprite _healthSpr;
    [SerializeField] Sprite _healSpr;
    [SerializeField] Sprite _defenseSpr;

    List<GrowthSlot> _growthSlots = new();
    int _growthSlotsCount = 0;

    float _lastSpacing = 300f;

    protected override void AwakeInternal()
    {
        _backButton.onClick.AddListener(OnClickBackButton);
        _panel.OnUnlockGrowthAction += OnUnlockGrowth;

        List<GrowthInfoEntry> entries = GameManager.Instance.GrowthInfoSetp;

        RectTransform slotRect = _slotOrigin.GetComponent<RectTransform>();
        float slotHeight = slotRect.sizeDelta.x + _layoutGroup.spacing;

        _growthSlotsCount = 0;

        for (int i = 0; i < entries.Count; ++i)
        {
            for (int j = 0; j < entries[i].GrowthInfos.Count; ++j)
            {
                GrowthSlot newSlot = Instantiate(_slotOrigin);
                newSlot.transform.SetParent(_content);

                SlotInfo slotInfo = new SlotInfo
                {
                    isCountable = false,
                };

                switch (entries[i].GrowthInfos[j].StatType)
                {
                    case StatType.Health:
                        slotInfo.contentSpr = _healthSpr;
                        break;

                    case StatType.Heal:
                        slotInfo.contentSpr = _healSpr;
                        break;

                    case StatType.Attack:
                        slotInfo.contentSpr = _attackSpr;
                        break;

                    case StatType.Defense:
                        slotInfo.contentSpr = _defenseSpr;
                        break;
                }

                _growthSlotsCount += 1; // 슬롯 해금 번호는 1부터
                newSlot.SetSlot(slotInfo, entries[i].GrowthInfos[j], _growthSlotsCount);
                _growthSlots.Add(newSlot);

                newSlot.OnClickGrowthButtonAction += OnClickGrowthSlot;
            }

        }

        RectTransform spacing = Instantiate(_spacing, _content);
        spacing.sizeDelta = new Vector2(spacing.sizeDelta.x, spacing.sizeDelta.y + _lastSpacing);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);

    }


    void OnClickGrowthSlot(GrowthSlot slot, int unlcokCount)
    {
        _panel.transform.position = slot.transform.position;
        _panel.Open(slot);
    }

    void OnClickBackButton()
    {
        _panel.Close();
    }

    void OnUnlockGrowth(int unlockCount)
    {
        // 번호 확인하고 아이콘 띄우기

        if (_growthSlots.Count <= unlockCount) return;


        if (_growthSlots[unlockCount].GrowthInfo.UnLockableLevel <= PlayerManager.Instance.Condition.GlobalLevel.Level)
        {
            _growthSlots[unlockCount].ShowUnlockableIcon();
        }

    }

}
