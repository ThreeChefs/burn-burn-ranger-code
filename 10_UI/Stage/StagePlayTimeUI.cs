using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StagePlayTimeUI : MonoBehaviour
{
    TextMeshProUGUI text;
    
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (StageManager.Instance == null) return;
        TimeSpan t = TimeSpan.FromSeconds(StageManager.Instance.PlayTime);
        text.text = t.ToString(@"mm\:ss");
    }
}
