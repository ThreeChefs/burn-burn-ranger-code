using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : BaseUI
{
    [SerializeField] private SkillSelectButton[] _skillButtons;


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
