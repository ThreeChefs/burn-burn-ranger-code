using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconPanel : MonoBehaviour
{
    [SerializeField] private bool _isActiveSkillView;
    [SerializeField] List<SkillSlot> _skillSlots;


    private void OnEnable()
    {
        int maxSkillCount = _skillSlots.Count;
        int skillCount = 0;

        for(int i = 0; i < _skillSlots.Count; i++)
        {
            _skillSlots[i]?.SetEmpty();
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


    protected virtual void SetSkillElement(int count, BaseSkill skill)
    {
        if (_skillSlots.Count <= count)
        {
            return;
        }

        if (_skillSlots[count] !=null)
        {
            _skillSlots[count].SetSkillElement(skill);
        }
    }


}
