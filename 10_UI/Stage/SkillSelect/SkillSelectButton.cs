using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _headerImg;
    [SerializeField] private Image _iconImg;

    private Button _button;

    private SkillSelectUI _skillSelectUI;
    private SkillSelectDto _nowSkillData;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectSkill);
        _skillSelectUI = GetComponentInParent<SkillSelectUI>();
    }

    public void SetSkillButton(SkillSelectDto skillData)
    {
        _nowSkillData = skillData;

        _nameText.text = skillData.Name;
        _descriptionText.text = skillData.Description;
        _iconImg.sprite = skillData.Icon;

        // todo : 헤더 이미지 색 달라지기
        switch (skillData.Type)
        {
            case SkillType.Active:
                break;
            case SkillType.Passive:
                break;
            case SkillType.Combination:
                break;
        }
    }

    void SelectSkill()
    {
        StageManager.Instance.SkillSystem.TrySelectSkill(_nowSkillData.Id);

        Destroy(_skillSelectUI.gameObject);
    }
}