using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : PopupUI
{
    [Title("Stage Info")]
    [SerializeField] private TextMeshProUGUI _stageNumText;

    [Title("Buttons")]
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _backButton;


    [Title("Scroll Panel")]
    [SerializeField] StageSelectPanel _stageSelectPanel;

    public event Action<int> OnSelectStageEvent;



    // todo : 나중에는 제일 마지막에 플레이한 stage 체크하기
    private int _nowSelectedStage = 1;

    private void Awake()
    {
        _selectButton.onClick.AddListener(OnClickSelectButton);
        _backButton.onClick.AddListener(OnClickBackButton);

        _stageSelectPanel.OnSnapAction += SetStageNum;
        
    }

    private void Start()
    {
        
        SetStageNum(_nowSelectedStage);
    }

    public void SetStageNum(int stageNum)
    {
        _nowSelectedStage  = stageNum + 1;
        _stageNumText.text = GameManager.Instance.StageDatabase[_nowSelectedStage - 1].StageName;

    }
    
    void OnClickSelectButton()
    {
        GameManager.Instance.StageProgress.SaveLastSelectedStage(_nowSelectedStage);
        OnSelectStageEvent?.Invoke(_nowSelectedStage);
        gameObject.SetActive(false);
    }

    void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
