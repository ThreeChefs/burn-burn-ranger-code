using UnityEngine;
using UnityEngine.UI;

public class StageUI : BaseUI
{
    [SerializeField] BigIntText _killCountText;
    [SerializeField] BigIntText _goldText;
    [SerializeField] Button _pauseButton;

    private void Start()
    {
        StageManager.Instance.AddKillCountAction += SetKillCount;
        
        _pauseButton.onClick.AddListener(OnClickPauseButton);

        StageProgressBarUI progressBar = (StageProgressBarUI)UIManager.Instance.LoadUI(UIName.UI_StageProgressBar);
        UIManager.Instance.LoadUI(UIName.UI_StagePause, false);
        SkillSelectUI skillSelectUI = (SkillSelectUI)UIManager.Instance.LoadUI(UIName.UI_SkillSelect, false);
        UIManager.Instance.LoadUI(UIName.UI_FortuneBox, false);


        skillSelectUI.OnCloseAction += progressBar.NextExp;


    }

    private void Update()
    {
        // todo: event로 추가되면 이벤트로 바꾸기
        SetGoldCount(PlayerManager.Instance.StagePlayer.GoldValue);
    }

    void SetKillCount(int count)
    {
        _killCountText.SetValue(count);
    }
    
    void SetGoldCount(int count)
    {
        _goldText.SetValue(count);
    }

    void OnClickPauseButton()
    {
        UIManager.Instance.ShowUI(UIName.UI_StagePause);
        StageManager.Instance.PauseGame();
    }
    
}
