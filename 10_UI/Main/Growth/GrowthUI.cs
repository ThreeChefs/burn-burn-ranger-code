using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthUI : BaseUI
{
    [Title("UI")]
    [SerializeField] RectTransform _content;
    [SerializeField] VerticalLayoutGroup _layoutGroup;
    [SerializeField] Transform _empty;

    [Title("프리팹")]
    [SerializeField] GrowthSlot _slotOrigin;

    [Title("Sprite")]
    [SerializeField] Sprite _attackSpr;
    [SerializeField] Sprite _healthSpr;
    [SerializeField] Sprite _healSpr;
    [SerializeField] Sprite _defenseSpr;


    List<GrowthSlot> growthSlots = new List<GrowthSlot>();



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

                newSlot.SetSlot(slotInfo, entries[i].GrowthInfos[j]);
                growthSlots.Add(newSlot);
                slotCount += 1;
            }

        }

        Instantiate(_empty, _content);
        //_line.localScale = new Vector3(
        //    slotCount * 10,
        //    _line.localScale.y,
        //    _line.localScale.z
        //    );

        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
    }


}
