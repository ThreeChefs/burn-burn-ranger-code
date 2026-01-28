
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : PopupUI
{
    [Title("SkillSelect UI")]
    [SerializeField] private SkillSelectButton[] _skillButtons;
    [SerializeField] private MeatItemButton _meatItemButton;
    
    void OnEnable()
    {
        List<SkillSelectDto> skills = StageManager.Instance.SkillSystem.GetSelectableSkills(Define.SelectableSkillMaxCount);

        for (int i = 0; i < _skillButtons.Length; i++)
        {
            _skillButtons[i].gameObject.SetActive(false);
        }

        _meatItemButton.gameObject.SetActive(false);

        SetSkillData(skills);
    }
    
    public void SetSkillData(List<SkillSelectDto> skillDatas)
    {
        if(skillDatas == null || skillDatas.Count == 0)
        {
            ShowMeatItemSlot();
            return;
        }


        for (int i = 0; i < _skillButtons.Length; i++)
        {
            if (skillDatas.Count > i)
            {
                _skillButtons[i].gameObject.SetActive(true);
                _skillButtons[i].SetSkillButton(skillDatas[i]);
            }
        }
    }

    public void ShowMeatItemSlot()
    {

        _meatItemButton.gameObject.SetActive(true);

    }

}
