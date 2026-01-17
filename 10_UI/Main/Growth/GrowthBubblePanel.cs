using DG.Tweening;
using Sirenix.OdinInspector;
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

    public void Open(GrowthInfo info, bool showButton)
    {
        _headerText.text = StatTypeText.StatName[info.StatType];
        _descText.text = StatTypeText.StatDescriptionName[info.StatType];
        _subDescText.text = StatTypeText.StatGrowthDesc[info.StatType];
        _goldText.text = info.GrowthPrice.ToString();
        _valueText.text = info.Value.ToString();

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

    public void Close()
    {
        this.transform.DOScale(Vector3.zero, _openDuration).SetEase(Ease.InCirc).OnComplete(Hide);
    }

    public void Hide()
    {
        this.transform.gameObject.SetActive(false);
    }
}
