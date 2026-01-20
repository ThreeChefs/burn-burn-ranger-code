using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템 효과 정보를 담는 UI
/// </summary>
public class ItemEffectUI : MonoBehaviour
{
    [SerializeField] private Image _skillColor;
    [SerializeField] private Outline _skillColorOutline;
    [SerializeField] private Image _skillLock;
    [SerializeField] private TextMeshProUGUI _skillDescription;

    public void SetData(EquipmentEffectData equipmentData, bool isLock)
    {
        _skillColor.color = ItemUtils.GetClassColor(equipmentData.UnlockClass);
        _skillColorOutline.effectColor = ItemUtils.GetHighlightColor(equipmentData.UnlockClass);
        _skillLock.gameObject.SetActive(isLock);
        _skillDescription.text = equipmentData.Description;
        _skillDescription.transform.parent.gameObject.SetActive(true);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _skillColor = transform.FindChild<Image>("Image - Class");
        _skillColorOutline = transform.FindChild<Outline>("Image - Class");
        _skillLock = transform.FindChild<Image>("Image - Lock");
        _skillDescription = transform.FindChild<TextMeshProUGUI>("Text (TMP) - SkillDescription");
    }
#endif
}
