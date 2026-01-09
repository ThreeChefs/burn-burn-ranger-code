using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _stageSelectButton;

    private void Awake()
    {

        _playButton.onClick.AddListener(OnClickPlayButton);
        _stageSelectButton.onClick.AddListener(OnClickStageSelectButton);

    }


    void OnClickPlayButton()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.StageScene);
    }

    void OnClickStageSelectButton()
    {
        UIManager.Instance.SpawnUI(UIName.UI_StageSelect);
    }
}
