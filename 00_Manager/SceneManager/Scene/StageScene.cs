using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance.LoadUI(UIName.UI_Stage);
    }
}
