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

    [SerializeField] private RectTransform _layoutGroupRect;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _expText;

    [SerializeField] private Button _nextButton;

    [SerializeField] private ResultRewardListPanel _rewardPanel;


    public void Start()
    {
        _nextButton.onClick.AddListener(NextScene);
    }

    public void Init(StageResultInfo stageResultInfo, List<StageRewardInfo> rewards = null)
    {
        _stageNumberText.text = stageResultInfo.stageName;
        
        _killCountText.text = stageResultInfo.killCount.ToString();

        // 플레이 시간 표시
        TimeSpan t = TimeSpan.FromSeconds(StageManager.Instance.PlayTime);
        _timeText.text = t.ToString(@"mm\:ss");

        // 보상 골드, 경험치 표시
        _goldText.text = stageResultInfo.gold.ToString();
        _expText.text = stageResultInfo.exp.ToString();

        _rewardPanel.SetRewardList(rewards);


        // 게임오브젝트가 꺼져있을 때 하는 Init / 레이아웃 정렬이 안맞을 수 있으니 Rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroupRect);
    }




    void NextScene()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }

}

