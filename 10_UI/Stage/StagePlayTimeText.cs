using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StagePlayTimeText : MonoBehaviour
{
    TextMeshProUGUI text;

    int _prevSec = -1;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (StageManager.Instance == null) return;

        int nowSec = (int)StageManager.Instance.PlayTime;

        if (nowSec == _prevSec) return;
        _prevSec = nowSec;

        int minutes = nowSec / 60;
        int seconds = nowSec % 60;

        text.text = $"{minutes:00} : {seconds:00}";
    }
}
