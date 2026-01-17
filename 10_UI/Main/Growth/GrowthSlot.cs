using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
public class GrowthSlot : BaseSlot
{
    [Title("성장 슬롯")]
    [SerializeField] Button _slotButton;

    [Title("흑백 Material")]
    [SerializeField] Material _grayScaleMat;

    public GrowthInfo GrowthInfo => _growthInfo;
    GrowthInfo _growthInfo;

    public int UnlockCount { get; private set;  }

    public event Action<GrowthSlot, int> OnClickGrowthButtonAction;


    Image[] _images;
    

    private void Awake()
    {
        _slotButton.onClick.AddListener(OnClickSlot);

        _images= this.GetComponentsInChildren<Image>();
        
    }

    public void SetSlot(SlotInfo slotInfo, GrowthInfo growthInfo, int unlockCount )
    {
        SetSlot(slotInfo);
        _growthInfo = growthInfo;
        UnlockCount = unlockCount;

        if(GameManager.Instance.GrowthProgress.NormalUnlockCount > unlockCount -1)
        {
            SetLockImg(false);
        }
        else
        {
            SetLockImg(true);
        }
    }

    public void OnClickSlot()
    {
        OnClickGrowthButtonAction?.Invoke(this, UnlockCount);
    }

    public void SetLockImg(bool locked)
    {
        for (int i = 0; i < _images.Length; i++)
        {
            if (locked)
                _images[i].material = _grayScaleMat;
            else
                _images[i].material = null;
        }
    }

}
