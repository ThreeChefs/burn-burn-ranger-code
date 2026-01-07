using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI _stageNumberText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Button _nextButton;
    
    public void Start()
    {
        _stageNumberText.text = StageManager.Instance.NowStageNumber.ToString();
        _killCountText.text = StageManager.Instance.KillCount.ToString();
        
        TimeSpan t = TimeSpan.FromSeconds(StageManager.Instance.PlayTime);
        _timeText.text = t.ToString(@"mm\:ss");
        
        _nextButton.onClick.AddListener(NextScene);
    }

    void NextScene()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }
}
