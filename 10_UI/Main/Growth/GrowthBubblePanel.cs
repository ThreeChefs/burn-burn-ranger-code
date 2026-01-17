using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrowthBubblePanel : MonoBehaviour
{
    [Title("LayoutGroup")]
    [SerializeField] RectTransform _bubble;
    [SerializeField] RectTransform _descRect;
    [SerializeField] RectTransform _goldRect;

    [Title("Text")]
    [SerializeField] TextMeshProUGUI _headerText;
    [SerializeField] TextMeshProUGUI _descText;
    [SerializeField] TextMeshProUGUI _subDescText;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] TextMeshProUGUI _goldText;

    [Title("Button")]
    [SerializeField] Button _unlockBtn;

    float _openDuration = 0.2f;
    float _positionOffset = 60f;
    GrowthSlot _targetSlot;

    public event Action<int> OnUnlockGrowthAction;


    private void Awake()
    {
        _unlockBtn.onClick.AddListener(OnClickUnlock);
    }

    public void Open(GrowthSlot slot)
    {
        _targetSlot = slot;

        _headerText.text = StatTypeText.StatName[slot.GrowthInfo.StatType];
        _descText.text = StatTypeText.StatDescriptionName[slot.GrowthInfo.StatType];
        _subDescText.text = StatTypeText.StatGrowthDesc[slot.GrowthInfo.StatType];
        _goldText.text = slot.GrowthInfo.GrowthPrice.ToString();
        _valueText.text = slot.GrowthInfo.Value.ToString();

        if (GameManager.Instance.GrowthProgress.NormalUnlockCount == slot.UnlockInfo.unlockCount - 1
            && slot.UnlockInfo.unlockLevel <= PlayerManager.Instance.Condition.GlobalLevel.Level)
        {
            _unlockBtn.gameObject.SetActive(true);
        }
        else
        {
            _unlockBtn.gameObject.SetActive(false);
        }


        this.gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_descRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_goldRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_bubble);

        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, _openDuration).SetEase(Ease.InCirc);
    }

    private void Update()
    {
        if (_targetSlot != null)
            this.transform.position = _targetSlot.transform.position + new Vector3(0, _positionOffset, 0);
    }


    public void Close()
    {
        _unlockBtn.interactable = false;
        this.transform.DOScale(Vector3.zero, _openDuration).SetEase(Ease.InCirc).OnComplete(Hide);
    }

    public void Hide()
    {
        this.transform.gameObject.SetActive(false);
        _unlockBtn.interactable = true;
    }

    public void OnClickUnlock()
    {
        if (PlayerManager.Instance.Wallet[WalletType.Gold].TryUse(_targetSlot.GrowthInfo.GrowthPrice))
        {
            GameManager.Instance.GrowthProgress.UnlockNormalGrowth(_targetSlot.UnlockInfo.unlockCount);

            _unlockBtn.gameObject.SetActive(false);

            _targetSlot.HideUnlockableIcon();
            _targetSlot.SetLockImg(false);
            OnUnlockGrowthAction?.Invoke(_targetSlot.UnlockInfo.unlockCount);

            Close();
        }
    }
}
