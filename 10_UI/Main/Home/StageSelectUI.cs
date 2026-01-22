using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : PopupUI
{
    [Title("Stage Info")]
    [SerializeField] private RectTransform _stageLabel;
    [SerializeField] private TextMeshProUGUI _stageNumText;

    [Title("Buttons")]
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _backButton;


    [Title("Scroll Panel")]
    [SerializeField] StageSelectPanel _stageSelectPanel;

    public event Action<int> OnSelectStageEvent;


    private int _nowSelectedStageIndex = 1;

    private void Awake()
    {
        _selectButton.onClick.AddListener(OnClickSelectButton);
        _backButton.onClick.AddListener(OnClickBackButton);

        _stageSelectPanel.OnSnapAction += SetStageInfo;
        _stageSelectPanel.OnDragStartAction += HideButton;

    }

    public override void OpenUIInternal()
    {
        base.OpenUIInternal();
        _nowSelectedStageIndex = GameManager.Instance.StageProgress.LastSelectedStageNum - 1;
        _stageSelectPanel.SetFocusContent(_nowSelectedStageIndex);
        SetStageInfo(_nowSelectedStageIndex);
    }

    public void SetStageInfo(int stageNum)
    {
        ShowButton(stageNum < GameManager.Instance.StageProgress.ClearStageNum);
        _nowSelectedStageIndex  = stageNum;
        _stageNumText.text = GameManager.Instance.StageDatabase[_nowSelectedStageIndex].StageName;
    }
    
    void OnClickSelectButton()
    {
        GameManager.Instance.StageProgress.SaveLastSelectedStage(_nowSelectedStageIndex + 1);
        OnSelectStageEvent?.Invoke(_nowSelectedStageIndex + 1);
        gameObject.SetActive(false);
    }

    void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }

    void HideButton()
    {
        _selectButton.interactable = false;

        _selectButton.transform.DOScale(0, 0.2f).SetUpdate(true);
        _stageLabel.DOScale(0, 0.2f).SetUpdate(true);
    }

    void ShowButton(bool playable)
    {
        if (playable)
        {
            _selectButton.transform.DOScale(1, 0.2f).SetUpdate(true).OnComplete(() => { _selectButton.interactable = true; });

        }
        
        
        _stageLabel.DOScale(1, 0.2f).SetUpdate(true);
    }


}
