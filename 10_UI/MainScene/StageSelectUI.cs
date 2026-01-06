using TMPro;
using UnityEngine;
using UnityEngine.UI;


// todo 탕탕특공대처럼 하려면 Play 버튼이랑 분리 되어야 함. 일단 기능 먼저 구현용
public class StageSelectUI : BaseUI
{
    [SerializeField] private int _maxStage = 100;     // 어디선가 가져와야함
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private TextMeshProUGUI _stageNumText;

    // todo : 나중에는 제일 마지막에 플레이한 stage 체크하기
    private int _nowSelectedStage = 1;

    private void Awake()
    {
        _nextButton.onClick.AddListener(OnClickNextButton);
        _prevButton.onClick.AddListener(OnClickPrevButton);
        _playButton.onClick.AddListener(OnClickPlayButton);
        
        _stageNumText.text = _nowSelectedStage.ToString();
    }


    void OnClickNextButton()
    {
        _nowSelectedStage = Mathf.Clamp(_nowSelectedStage + 1, 1, _maxStage);
        _stageNumText.text = _nowSelectedStage.ToString();
    }

    void OnClickPrevButton()
    {
        _nowSelectedStage = Mathf.Clamp(_nowSelectedStage - 1, 1, _maxStage);
        _stageNumText.text = _nowSelectedStage.ToString();
    }

    void OnClickPlayButton()
    {
        GameManager.Instance.SetSelectedStage(_nowSelectedStage);
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.StageScene);
    }

    public override void OpenUIInternal()
    {
        
    }

    public override void CloseUIInternal()
    {
        
    }
}
