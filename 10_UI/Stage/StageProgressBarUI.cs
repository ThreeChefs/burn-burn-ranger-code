using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageProgressBarUI : BaseUI
{
    [SerializeField] LevelPanel _levelPanel;

    public void NextExp(BaseUI ui)
    {
        _levelPanel.SetNextExpValue();
    }

}
