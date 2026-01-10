using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePauseUI : PopupUI
{
    [SerializeField] private Button _backButton;

    private void Awake()
    {
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    void OnClickBackButton()
    {
       UIManager.Instance.CloseUI(UIName.UI_StagePause);
       StageManager.Instance.ResumeGame();
    }

}
