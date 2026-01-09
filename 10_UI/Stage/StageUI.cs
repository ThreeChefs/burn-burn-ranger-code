using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUI : BaseUI
{
    [SerializeField] BigIntText _killCountText;
    [SerializeField] BigIntText _goldText;

    private void Start()
    {
        StageManager.Instance.AddKillCountAction += SetKillCount;
        
        //PlayerManager.Instance.StagePlayer.GoldValue
        
        UIManager.Instance.LoadUI(UIName.UI_StageProgressBar);
        UIManager.Instance.LoadUI(UIName.UI_SkillSelect, false);
    }

    private void Update()
    {
        // todo event로 추가되면 이벤트로 바꾸기
        SetGoldCount(PlayerManager.Instance.StagePlayer.GoldValue);
    }

    void SetKillCount(int count)
    {
        _killCountText.SetValue(count);
    }
    
    void SetGoldCount(int count)
    {
        _goldText.SetValue(count);
    }
    
}
