using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _headerImg;
    [SerializeField] private Image _iconImg;
    
    Button _button;
    
    SkillSelectUI _skillSelectUI;

    private void Start()
    {
        _button = GetComponent<Button>();
        _skillSelectUI = GetComponentInParent<SkillSelectUI>();
    }

    public void SetSkillButton(SkillSelectDto skillData)
    {
        _skillSelectUI.CloseUI();
    }
    
    
    
    
    
}
