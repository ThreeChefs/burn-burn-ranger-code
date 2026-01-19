using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePauseUI : PopupUI
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _homeButton;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnClickBackButton);
        _homeButton.onClick.AddListener(OnClickHomeButton); 
    }

    void OnClickBackButton()
    {
       UIManager.Instance.CloseUI(UIName.UI_StagePause);
       StageManager.Instance.ResumeGame();
    }

    void OnClickHomeButton()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }

}
