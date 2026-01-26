using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _stageSelectButton;
    [SerializeField] private TextMeshProUGUI _stageName;

    Image _stageSelectButtonImg;
    StageSelectUI _stageSelectUI;

    private void Awake()
    {
        _playButton.onClick.AddListener(OnClickPlayButton);
        _stageSelectButton.onClick.AddListener(OnClickStageSelectButton);

        _stageSelectButtonImg = _stageSelectButton.GetComponent<Image>();
    }

    private void Start()
    {
        _stageSelectUI = (StageSelectUI)UIManager.Instance.LoadUI(UIName.UI_StageSelect, false);
        _stageSelectUI.OnSelectStageEvent += SetStageSelectButtonImg;

        List<StageData> stageData = GameManager.Instance.StageDatabase;
        if (stageData == null || stageData.Count == 0)
            return;

        int requestedIndex = GameManager.Instance.StageProgress.LastSelectedStageNum - 1;
        int stageIndex = Mathf.Clamp(requestedIndex, 0, stageData.Count - 1);

        _stageName.text = stageData[stageIndex].StageName;
        _stageSelectButtonImg.sprite = stageData[stageIndex].StageIcon;

    }


    void OnClickPlayButton()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.StageScene);
    }

    void OnClickStageSelectButton()
    {
        UIManager.Instance.ShowUI(UIName.UI_StageSelect);
    }

    public void SetStageSelectButtonImg(int stageNum)
    {
        if (stageNum - 1 < 0 || stageNum - 1 >= GameManager.Instance.StageDatabase.Count)
        {
            return;
        }

        _stageSelectButtonImg.sprite = GameManager.Instance.StageDatabase[stageNum - 1].StageIcon;
        _stageName.text = GameManager.Instance.StageDatabase[stageNum - 1].StageName;
    }
}
