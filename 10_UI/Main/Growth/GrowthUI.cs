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

    List<GrowthSlot> growthSlots = new List<GrowthSlot>();
    float lastSpacing = 300f;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    public void Start()
    {
        List<GrowthInfoEntry> entries = GameManager.Instance.GrowthInfoSetp;

        RectTransform slotRect = _slotOrigin.GetComponent<RectTransform>();
        float slotHeight = slotRect.sizeDelta.x + _layoutGroup.spacing;

        int slotCount = 0;

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

                newSlot.SetSlot(slotInfo, entries[i].GrowthInfos[j], slotCount);
                growthSlots.Add(newSlot);
                newSlot.OnClickGrowthButtonAction += OnClickGrowthSlot;

                slotCount += 1;
            }

        }

        RectTransform spacing = Instantiate(_spacing, _content);
        spacing.sizeDelta = new Vector2(spacing.sizeDelta.x, spacing.sizeDelta.y + lastSpacing);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);

    }


    void OnClickGrowthSlot(GrowthSlot slot, int unlcokCount)
    {
        _panel.transform.position = slot.transform.position;
        _panel.Open(slot, true);

    }

    void OnClickBackButton()
    {
        _panel.Close();
    }

}
