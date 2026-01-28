using UnityEngine;
using UnityEngine.UI;

public class StagePauseUI : PopupUI
{
    //[Title("StagePause UI")]
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _damageDataButton;


    protected override void AwakeInternal()
    {
        base.AwakeInternal();

        UIManager.Instance.LoadUI(UIName.UI_Settings,false); // 이 UI에서 추가로 쓸거니까
        UIManager.Instance.LoadUI(UIName.UI_SkillDamageData,false)  ;

        _backButton.onClick.AddListener(OnClickBackButton);
        _homeButton.onClick.AddListener(OnClickHomeButton);
        _settingButton.onClick.AddListener(OnClickSettingButton);
        _damageDataButton.onClick.AddListener(OnClickDamageDataButton);

    }


    void OnClickBackButton()
    {
        CloseUI();
        StageManager.Instance.ResumeGame();
    }

    void OnClickHomeButton()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }

    void OnClickSettingButton()
    {
        UIManager.Instance.ShowUI(UIName.UI_Settings);
    }

    void OnClickDamageDataButton()
    {
        UIManager.Instance.ShowUI(UIName.UI_SkillDamageData);
    }
}
