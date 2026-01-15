using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultUI : PopupUI
{
    [SerializeField] private TextMeshProUGUI _stageNumberText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _expText;

    [SerializeField] private Button _nextButton;

    [SerializeField] private ResultRewardListPanel rewardPanel;


    public void Start()
    {
        _stageNumberText.text = (StageManager.Instance.NowStageNumber + 1).ToString();
        _killCountText.text = StageManager.Instance.KillCount.ToString();

        TimeSpan t = TimeSpan.FromSeconds(StageManager.Instance.PlayTime);
        _timeText.text = t.ToString(@"mm\:ss");

        _nextButton.onClick.AddListener(NextScene);
    }

    public void Init(int gold, int exp, List<StageRewardInfo> rewards = null)
    {
        // 보상 골드, 경험치 표시
        _goldText.text = gold.ToString();
        _expText.text = exp.ToString();

       
    }




    void NextScene()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }

}