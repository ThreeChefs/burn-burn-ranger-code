using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDamageDataUI : PopupUI
{
    [SerializeField] Button _backButton;

    protected override void AwakeInternal()
    {
        base.AwakeInternal();
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    void OnClickBackButton()
    {
        CloseUI();
    }
}
