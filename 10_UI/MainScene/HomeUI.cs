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

        // todo : 마지막으로 플레이한 스테이지로 변경
        _stageName.text = GameManager.Instance.StageDatabase[0].StageName;
        _stageSelectButtonImg.sprite = GameManager.Instance.StageDatabase[0].StageIcon;
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
        if(stageNum-1 < 0 || stageNum-1 >= GameManager.Instance.StageDatabase.Count)
        {
            return;
        }

        _stageSelectButtonImg.sprite = GameManager.Instance.StageDatabase[stageNum-1].StageIcon;
        _stageName.text = GameManager.Instance.StageDatabase[stageNum - 1].StageName;
    }
}
