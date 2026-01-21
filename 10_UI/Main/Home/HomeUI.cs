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

        int lastStageIndex = GameManager.Instance.StageProgress.LastSelectedStageNum - 1;
        lastStageIndex = Mathf.Clamp(lastStageIndex, 0, lastStageIndex);
        if (0 <= lastStageIndex && lastStageIndex < GameManager.Instance.StageDatabase.Count)
        {
            _stageName.text = GameManager.Instance.StageDatabase[lastStageIndex].StageName;
            _stageSelectButtonImg.sprite = GameManager.Instance.StageDatabase[lastStageIndex].StageIcon;
        }
        else if (GameManager.Instance.StageDatabase.Count <= lastStageIndex)
        {
            _stageName.text = GameManager.Instance.StageDatabase[GameManager.Instance.StageDatabase.Count - 1].StageName;
            _stageSelectButtonImg.sprite = GameManager.Instance.StageDatabase[GameManager.Instance.StageDatabase.Count - 1].StageIcon;
        }

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
