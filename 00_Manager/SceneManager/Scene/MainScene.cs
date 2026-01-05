using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance?.SpawnUI(UIPanelType.UI_StageSelect);
        
    }
}
