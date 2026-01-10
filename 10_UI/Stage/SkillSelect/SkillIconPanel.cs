using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconPanel : MonoBehaviour
{
    [SerializeField] private bool _isActiveSkillView;

    [TableList(ShowIndexLabels = true)]
    [SerializeField] List<SkillIconPanelElement> _panelElements;


    private void OnEnable()
    {
        int maxSkillCount = _panelElements.Count;
        int skillCount = 0;

        for(int i = 0; i < _panelElements.Count; i++)
        {
            _panelElements[i].Icon.gameObject.SetActive(false);
            if(_panelElements[i].SkillLevelPanel != null)
                _panelElements[i].SkillLevelPanel.gameObject.SetActive(false);
        }

        foreach (BaseSkill skill in StageManager.Instance.SkillSystem.OwnedSkills.Values)
        {
            
            if(_isActiveSkillView)
            {
                if(skill.SkillData.Type==SkillType.Active ||
                    skill.SkillData.Type == SkillType.Combination)
                {
                    SetSkillElement(skillCount, skill);
                    skillCount++;
                }
            }
            else
            {
                if (skill.SkillData.Type == SkillType.Passive)
                {
                    SetSkillElement(skillCount, skill);
                    skillCount++;
                }

            }
            
            if(skillCount == maxSkillCount)
            {
                break;
            }

        }
    }


    void SetSkillElement(int count, BaseSkill skill)
    {
        if(_panelElements.Count <= count)
        {
            return;
        }

        if(_panelElements[count].Icon != null)
        {
            _panelElements[count].Icon.sprite = skill.SkillData.Icon;
            _panelElements[count].Icon.gameObject.SetActive(true);
        }

        if (_panelElements[count].SkillLevelPanel != null)
        {
            _panelElements[count].SkillLevelPanel.Init(skill.SkillData.Type, skill.CurLevel, false);
            _panelElements[count].SkillLevelPanel.gameObject.SetActive(true);
        }
    }


}

[System.Serializable]
public struct SkillIconPanelElement
{
    public Image Icon;
    public SkillLevelPanel SkillLevelPanel;
}
