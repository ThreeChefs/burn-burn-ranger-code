using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUI : BaseUI
{
    [SerializeField] TextMeshProUGUI killCountText;

    private void Start()
    {
        StageManager.Instance.AddKillCountAction += SetKillCount;
        UIManager.Instance.LoadUI(UIName.UI_StageProgressBar);
        UIManager.Instance.LoadUI(UIName.UI_SkillSelect, false);
    }

    void SetKillCount(int count)
    {
        killCountText.text = count.ToString();
    }
    
}
