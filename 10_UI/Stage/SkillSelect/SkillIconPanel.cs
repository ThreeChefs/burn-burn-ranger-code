using Sirenix.OdinInspector.Editor.Drawers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconPanel : MonoBehaviour
{
    [SerializeField] private bool _isActiveSkillView;
    [SerializeField] private Image[] _icons;
   
    private void Start()
    {
        int maxSkillCount = _icons.Length;
        int skillCount = 0;

        for(int i = 0; i < _icons.Length; i++)
        {
            _icons[i].gameObject.SetActive(false);
        }

        foreach (BaseSkill skill in StageManager.Instance.SkillSystem.OwnedSkills.Values)
        {
            
            if(_isActiveSkillView)
            {
                if(skill.SkillData.Type==SkillType.Active ||
                    skill.SkillData.Type == SkillType.Combination)
                {
                    _icons[skillCount].sprite = skill.SkillData.Icon;
                    skillCount++;
                    _icons[skillCount - 1].gameObject.SetActive(true);
                }
            }
            else
            {
                if (skill.SkillData.Type == SkillType.Passive)
                {
                    _icons[skillCount].sprite = skill.SkillData.Icon;
                    skillCount++;
                    _icons[skillCount - 1].gameObject.SetActive(true);
                }

            }
            
            if(skillCount == maxSkillCount)
            {
                break;
            }

        }

    }
}
