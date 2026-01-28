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
    [SerializeField] SkillCombinationAlertPanel _combiAlertPanel;
    [SerializeField] GameObject _newText;


    [SerializeField] GameObject[] _skillTypePanel;

    private SkillSelectDto _nowSkillData;

    protected override void Awake()
    {
        base.Awake();
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


        if(skillData.CurLevel ==0)
            _newText.SetActive(true);
        else
            _newText.SetActive(false);




        // 돌파조합 표시
        if (skillData.CombinationIcons.Length > 0)
        {
            _combiAlertPanel.gameObject.SetActive(true);
            _combiAlertPanel.Init(skillData);
        }
        else
            _combiAlertPanel.gameObject.SetActive(false);


    }
    
    protected override void OnClick()
    {
        base.OnClick();
        // Select Skill
        StageManager.Instance.SkillSystem.TryAcquireSkill(_nowSkillData.Id);
        StageManager.Instance.ResumeGame();
        UIManager.Instance.CloseUI(UIName.UI_SkillSelect);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

}