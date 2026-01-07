using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectButton : BaseButton
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _headerImg;
    [SerializeField] private Image _iconImg;
    [SerializeField] SkillLevelPanel _skillLevelPanel;

    private SkillSelectUI _skillSelectUI;
    private SkillSelectDto _nowSkillData;

    protected override void Awake()
    {
        base.Awake();
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
        
        _skillLevelPanel.Init(skillData.Type,skillData.CurLevel);
        
    }
    
    protected override void OnClick()
    {
        // Select Skill
        StageManager.Instance.SkillSystem.TrySelectSkill(_nowSkillData.Id);
        StageManager.Instance.ResumeGame();
        Destroy(_skillSelectUI.gameObject);
    }

}