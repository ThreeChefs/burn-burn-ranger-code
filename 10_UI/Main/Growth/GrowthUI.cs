using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
                newSlot.transform.SetParent(_content, false);

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

                GrowthUnlockInfo unlockInfo = new GrowthUnlockInfo
                {
                    unlockCount = _growthSlotsCount,
                    unlockLevel = entries[i].UnlockLevel
                };

                newSlot.SetSlot(slotInfo, entries[i].GrowthInfos[j], unlockInfo);
                newSlot.SetLevelLabel(j == 0 ? entries[i].UnlockLevel : 0);
                _growthSlots.Add(newSlot);

                newSlot.OnClickGrowthButtonAction += OnClickGrowthSlot;
            }

        }

        RectTransform spacing = Instantiate(_spacing, _content);
        spacing.sizeDelta = new Vector2(spacing.sizeDelta.x, spacing.sizeDelta.y + _lastSpacing);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);

    }

    public override void OpenUIInternal()
    {
        base.OpenUIInternal();

        int unlockCount = GameManager.Instance.GrowthProgress.NormalUnlockCount;

        
        ScrollToSlot(Mathf.Min(unlockCount, _growthSlots.Count - 1),false);

    }

    void ScrollToSlot(int index, bool animated = true)
    {
        if (index < 0 || index >= _growthSlots.Count) return;
        
        Canvas.ForceUpdateCanvases();
        
        GrowthSlot slot = _growthSlots[index];
        RectTransform targetRect = slot.GetComponent<RectTransform>();
        RectTransform viewportRect = _content.parent as RectTransform;
        
        Vector3 itemWorldCenter = GetWidgetWorldPoint(targetRect);
        
        Vector3 viewportWorldCenter = GetWidgetWorldPoint(viewportRect);
        
        Vector3 itemPositionInContent = _content.InverseTransformPoint(itemWorldCenter);
        Vector3 viewportPositionInContent = _content.InverseTransformPoint(viewportWorldCenter);
        
        Vector3 difference = viewportPositionInContent - itemPositionInContent;

        difference.z = 0f;
        difference.x = 0f;
        
        // 위치 계산
        Vector2 newPosition = _content.anchoredPosition + new Vector2(difference.x, difference.y);
        
        // 끝에는 도달하지 않도록 클램핑
        float contentHeight = _content.rect.height;
        float viewportHeight = viewportRect.rect.height;
        float minY = -(contentHeight - viewportHeight);
        
        newPosition.y = Mathf.Clamp(newPosition.y, minY, 0);
        
        _content.DOKill();
        
        if (animated)
        {
            _content.DOAnchorPos(newPosition, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            _content.anchoredPosition = newPosition;
        }
    }
    
    // 위젯의 중앙 월드 위치를 계산 (pivot 고려)
    Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        // pivot offset을 고려하여 중앙 위치 계산
        Vector3 pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        Vector3 localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }


    void OnClickGrowthSlot(GrowthSlot slot, int unlcokCount)
    {
        ScrollToSlot(unlcokCount - 1);
        _panel.transform.position = slot.transform.position;
        _panel.Open(slot);
    }

    void OnClickBackButton()
    {
        _panel.Close();
    }

    void OnUnlockGrowth(int unlockCount)
    {
        if (_growthSlots.Count <= unlockCount) return;


        if (_growthSlots[unlockCount].UnlockInfo.unlockLevel <= PlayerManager.Instance.Condition.GlobalLevel.Level)
        {
            _growthSlots[unlockCount].ShowUnlockableIcon();
        }

    }

}
