using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatItemButton : BaseButton
{
    private float _healAmount = 20f;    // MeatItem이랑 동일하게 설정

    protected override void OnClick()
    {
        base.OnClick();

        StageManager.Instance.ResumeGame();
        UIManager.Instance.CloseUI(UIName.UI_SkillSelect);

        PlayerManager.Instance.StagePlayer.Condition[StatType.Health].Add(_healAmount);
    }
}
