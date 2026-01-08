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
    }

    public override void OpenUIInternal()
    {
        
    }

    public override void CloseUIInternal()
    {
    }

    void SetKillCount(int count)
    {
        killCountText.text = count.ToString();
    }
    
}
