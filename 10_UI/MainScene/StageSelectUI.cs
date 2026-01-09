using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// todo 탕탕특공대처럼 하려면 Play 버튼이랑 분리 되어야 함. 일단 기능 먼저 구현용
public class StageSelectUI : PopupUI
{
    [Title("Stage Info")]
    [SerializeField] private int _maxStage = 100;           // 어디선가 가져와야함
    [SerializeField] private TextMeshProUGUI _stageNumText;

    [Title("Buttons")]
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _backButton;


    [Title("Stage Icon")]
    [SerializeField] private Image _stageIconImg;
    [SerializeField] private Image _prevStageIconImg;
    [SerializeField] private Image _nextStageIconImg;



    // todo : 나중에는 제일 마지막에 플레이한 stage 체크하기
    private int _nowSelectedStage = 1;

    private void Awake()
    {
        
        _nextButton.onClick.AddListener(OnClickNextButton);
        _prevButton.onClick.AddListener(OnClickPrevButton);
        _selectButton.onClick.AddListener(OnClickPlayButton);
        _backButton.onClick.AddListener(OnClickBackButton);

        _stageNumText.text = _nowSelectedStage.ToString();

        SetStageIcon();

    }


    void SetStageIcon()
    {
        if(_nowSelectedStage == 1)
        {
            _prevButton.gameObject.SetActive(false);
            _prevStageIconImg.gameObject.SetActive(false);
        }

        if (_nowSelectedStage == _maxStage)
        {
            _nextButton.gameObject.SetActive(false);
            _nextStageIconImg.gameObject.SetActive(false);
        }

        if(_nowSelectedStage > 1 && _nowSelectedStage < _maxStage)
        {
            _prevButton.gameObject.SetActive(true);
            _nextButton.gameObject.SetActive(true);
            _prevStageIconImg.gameObject.SetActive(true);
            _nextStageIconImg.gameObject.SetActive(true);
        }
    }


    void OnClickNextButton()
    {
        _nowSelectedStage = Mathf.Clamp(_nowSelectedStage + 1, 1, _maxStage);
        _stageNumText.text = _nowSelectedStage.ToString();
        SetStageIcon();
    }

    void OnClickPrevButton()
    {
        _nowSelectedStage = Mathf.Clamp(_nowSelectedStage - 1, 1, _maxStage);
        _stageNumText.text = _nowSelectedStage.ToString();
        SetStageIcon();
    }

    void OnClickPlayButton()
    {
        GameManager.Instance.SetSelectedStage(_nowSelectedStage);
    }

    void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
