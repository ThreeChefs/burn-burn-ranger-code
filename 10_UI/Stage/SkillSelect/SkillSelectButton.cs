using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectButton : BaseButton
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _headerImg;
    [SerializeField] private Image _iconImg;
    [SerializeField] SkillLevelPanel _skillLevelPanels;

    [SerializeField] GameObject[] _skillTypePanel;

    private SkillSelectUI _skillSelectUI;
    private SkillSelectDto _nowSkillData;

    protected override void Awake()
    {
        base.Awake();
        _skillSelectUI = GetComponentInParent<SkillSelectUI>();
    }
    
    public void SetSkillButton(SkillSelectDto skillData)
    {
        foreach (GameObject skill in _skillTypePanel)
        {
            skill.SetActive(false);
        }

        _nowSkillData = skillData;

        _nameText.text = skillData.Name;
        _descriptionText.text = skillData.Description;
        _iconImg.sprite = skillData.Icon;

        switch (skillData.Type)
        {
            case SkillType.Active:
                _skillTypePanel[0].SetActive(true);
                break;
            case SkillType.Passive:
                _skillTypePanel[1].SetActive(true);
                break;
            case SkillType.Combination:
                _skillTypePanel[2].SetActive(true);
                break;
        }
        
        _skillLevelPanels.Init(skillData.Type,skillData.CurLevel);
        
    }
    
    protected override void OnClick()
    {
        // Select Skill
        StageManager.Instance.SkillSystem.TrySelectSkill(_nowSkillData.Id);
        StageManager.Instance.ResumeGame();
        UIManager.Instance.CloseUI(UIName.UI_SkillSelect);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if(transform.parent != null)
        {
            if (transform.parent.childCount <= 1)
                return;
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        }
    }

}