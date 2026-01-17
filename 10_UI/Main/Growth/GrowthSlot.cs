using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
public class GrowthSlot : BaseSlot
{
    [Title("성장 슬롯")]
    [SerializeField] Button _slotButton;

    public GrowthInfo GrowthInfo => _growthInfo;
    GrowthInfo _growthInfo;

    public int UnlockCount { get; private set;  }

    public event Action<GrowthSlot, int> OnClickGrowthButtonAction;


    private void Awake()
    {
        _slotButton.onClick.AddListener(OnClickSlot);
    }

    public void SetSlot(SlotInfo slotInfo, GrowthInfo growthInfo, int unlockCount )
    {
        SetSlot(slotInfo);
        _growthInfo = growthInfo;
        UnlockCount = unlockCount;
    }

    public void OnClickSlot()
    {
        OnClickGrowthButtonAction?.Invoke(this, UnlockCount);
    }

}
