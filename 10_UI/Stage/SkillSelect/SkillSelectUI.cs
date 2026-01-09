using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSelectUI : PopupUI
{
    [SerializeField] private SkillSelectButton[] _skillButtons;
    
    void OnEnable()
    {
        List<SkillSelectDto> skills = StageManager.Instance.SkillSystem.ShowSelectableSkills(Define.SelectableSkillMaxCount);
        SetSkillData(skills);
    }
    
    public void SetSkillData(List<SkillSelectDto> skillDatas)
    {
        for (int i = 0; i < _skillButtons.Length; i++)
        {
            if (skillDatas.Count > i)
            {
                _skillButtons[i].gameObject.SetActive(true);
                _skillButtons[i].SetSkillButton(skillDatas[i]);
            }
            else
            {
                _skillButtons[i].gameObject.SetActive(false);
            }
        }
    }

}
