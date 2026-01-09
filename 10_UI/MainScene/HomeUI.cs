using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [SerializeField] private Button _playButton;

    private void Awake()
    {

        _playButton.onClick.AddListener(OnClickPlayButton);

    }


    void OnClickPlayButton()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.StageScene);
    }


}
