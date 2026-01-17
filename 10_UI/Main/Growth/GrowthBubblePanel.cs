using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
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
    Transform _targetSlot;

    public void Open(GrowthSlot slot, bool showButton)
    {
        _targetSlot = slot.transform;

        _headerText.text = StatTypeText.StatName[slot.GrowthInfo.StatType];
        _descText.text = StatTypeText.StatDescriptionName[slot.GrowthInfo.StatType];
        _subDescText.text = StatTypeText.StatGrowthDesc[slot.GrowthInfo.StatType];
        _goldText.text = slot.GrowthInfo.GrowthPrice.ToString();
        _valueText.text = slot.GrowthInfo.Value.ToString();

        if(showButton)
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
            this.transform.position = _targetSlot.position + new Vector3(0, _positionOffset, 0);
    }


    public void Close()
    {
        this.transform.DOScale(Vector3.zero, _openDuration).SetEase(Ease.InCirc).OnComplete(Hide);
    }

    public void Hide()
    {
        this.transform.gameObject.SetActive(false);
    }
}
