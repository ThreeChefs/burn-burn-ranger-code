using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthUI : BaseUI
{
    [Title("UI")]
    [SerializeField] RectTransform _content;

    [Title("프리팹")]
    [SerializeField] GrowthSlot _slotOrigin;
    [SerializeField] GameObject _linePrefab;

    [Title("Sprite")]
    [SerializeField] Sprite _attackSpr;
    [SerializeField] Sprite _healthSpr;
    [SerializeField] Sprite _healSpr;
    [SerializeField] Sprite _defenseSpr;


    List<GrowthSlot> growthSlots = new List<GrowthSlot>();



    public void Start()
    {
        List<GrowthInfoEntry> entries = GameManager.Instance.GrowthInfoSetp;

        for (int i = 0; i < entries.Count; ++i)
        {

            for (int j = 0; j < entries[i].GrowthInfos.Count; ++j)
            {
                GrowthSlot newSlot = Instantiate(_slotOrigin);
                newSlot.transform.SetParent(_content);

                if (i < entries.Count - 1)
                {
                    Instantiate(_linePrefab, _content);
                }

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
            }



        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
    }


}
