using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : PopupUI
{
    [Title("Stage Info")]
    private int _maxStage = 100;           // 어디선가 가져와야함
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

    public event Action<int> OnSelectStageEvent;



    // todo : 나중에는 제일 마지막에 플레이한 stage 체크하기
    private int _nowSelectedStage = 1;

    private void Awake()
    {
        
        _nextButton.onClick.AddListener(OnClickNextButton);
        _prevButton.onClick.AddListener(OnClickPrevButton);
        _selectButton.onClick.AddListener(OnClickSelectButton);
        _backButton.onClick.AddListener(OnClickBackButton);


    }

    private void Start()
    {
        _maxStage = GameManager.Instance.StageDatabase.Count;

        
        SetStageIcon();
    }


    void SetStageIcon()
    {
        // 이전 스테이지, 다음 스테이지 아이콘 버튼 처리
        if(_nowSelectedStage-1 <= 0)
        {
            _prevButton.gameObject.SetActive(false);
            _prevStageIconImg.gameObject.SetActive(false);
        }
        else
        {
            _prevButton.gameObject.SetActive(true);

            _prevStageIconImg.sprite = GameManager.Instance.StageDatabase[_nowSelectedStage - 2].StageIcon;
            _prevStageIconImg.gameObject.SetActive(true);
        }

        if(_nowSelectedStage +1 > _maxStage)
        {
            _nextButton.gameObject.SetActive(false);
            _nextStageIconImg.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);

            _nextStageIconImg.sprite = GameManager.Instance.StageDatabase[_nowSelectedStage].StageIcon;
            _nextStageIconImg.gameObject.SetActive(true);
        }

        _stageNumText.text = GameManager.Instance.StageDatabase[_nowSelectedStage - 1].StageName;
        _stageIconImg.sprite = GameManager.Instance.StageDatabase[_nowSelectedStage - 1].StageIcon;

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

    void OnClickSelectButton()
    {
        GameManager.Instance.SetSelectedStage(_nowSelectedStage);
        OnSelectStageEvent?.Invoke(_nowSelectedStage);
        gameObject.SetActive(false);
    }

    void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
