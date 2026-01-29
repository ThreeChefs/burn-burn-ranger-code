using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FortuneBoxUI : PopupUI
{
    [Title("UI 구성")]
    [SerializeField] FortuneBoxSlot[] slots;
    [SerializeField] FortuneBoxButton _pickButton;
    [SerializeField] FortuneBoxButton _skipButton;
    [SerializeField] TextMeshProUGUI _buttonText;
    [SerializeField] Button _backButton;


    [Title("Focus 연출")]
    [SerializeField] float _stepInterval = 0.05f;   // 포커스 속도


    WaveClearRewardType _type = WaveClearRewardType.Fortune_Skill_Random;
    Coroutine _nowFocusRoutine;
    WaitForSecondsRealtime _intervalWait;
    WaitForSecondsRealtime _skipWaitTime = new WaitForSecondsRealtime(1.5f);


    static int _maxSlotcount = Define.FortuneBoxSkillSlotCount;
    
    const int _healAmount = 50;
    const int _goldAmount = 100;


    #region 연출 슬롯 순서

    // 빙글빙글 안, 밖, 안, 밖 으로 4x4 일때 
    // 이 순서로 5개일때 연속으로 있을 자리를 픽해가자
    int[] _spiralOrder = new int[]{ 0,1,2,3,7,11,15,14,13,12,8,4,5,6,10,9,
                                    //5,6,10,14,13,12,8,4,0,1,2,3,7,11,15,
                                    //14,13,12,8,4,0,1,2,3,7,11,10,9,5,6,
                                    //10,9,5,1,2,3,7,11,15,14,13,12,8,4
    };

    // 순서대로
    int[] _defaultOrder = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

    // 지그재그
    int[] _zigzagOrder = new int[] { 0, 1, 2, 3, 7, 6, 5, 4, 8, 9, 10, 11, 15, 14, 13, 12 };

    // 대각선 쭈르륵
    int[][] _diagonalOrder = new int[][]
    {
        new[] {0},
        new[] {1,4},
        new[] {2,5,8},
        new[] {3,6,9,12},
        new[] {7,10,13},
        new[] {11,14},
        new[] {15}
    };

    #endregion


    protected override void AwakeInternal()
    {
        base.AwakeInternal();
        _intervalWait = new WaitForSecondsRealtime(_stepInterval);

        _backButton.onClick.AddListener(CloseUI);

    }


    List<int> _pickSlotIdx = new List<int>();
    List<int> _orderSlotIdx = new List<int>();

    [Button("테스트")]
    public void Init(WaveClearRewardType type)
    {
        // 초기화
        _backButton.gameObject.SetActive(false);
        _skipButton.gameObject.SetActive(false);
        _pickButton.gameObject.SetActive(false);

        _orderSlotIdx.Clear();
        _pickSlotIdx.Clear();

        // 상자 타입 지정
        _type = type;

        // 랜덤이면 1, 3, 5 중 하나 지정
        if (type == WaveClearRewardType.Fortune_Skill_Random)
        {
            _type = (WaveClearRewardType)Define.Random.Next((int)WaveClearRewardType.Fortune_Skill_1
                , (int)WaveClearRewardType.Fortune_Skill_5 + 1);
        }


        List<SkillSelectDto> slotList = null;

        int pickNum = Define.Random.Next(0, _maxSlotcount);
        int[] targetOrder = _defaultOrder;


        SkillSystem system = StageManager.Instance.SkillSystem;

        int pickCount = 0;
        int actualCount = 0;

        switch (_type)
        {
            case WaveClearRewardType.Fortune_Skill_1:
                pickCount = 1;
                slotList = system.GetRolledSkills(pickCount, out actualCount);
                break;

            case WaveClearRewardType.Fortune_Skill_3:
                pickCount = 3;
                slotList = system.GetRolledSkills(pickCount, out actualCount);
                targetOrder = _spiralOrder;
                break;
            case WaveClearRewardType.Fortune_Skill_5:
                pickCount = 5;
                slotList = system.GetRolledSkills(pickCount, out actualCount);
                targetOrder = _zigzagOrder;
                break;
        }

        if (slotList == null || slotList.Count == 0)
        {
            // 음식 뽑기로 세팅
            for (int i = 0; i < slots.Length; ++i)
            {
                if(i%2 ==0)
                {
                    slots[i].SetSlot(FortuneBoxSlotType.Food);
                }
                else
                    slots[i].SetSlot(FortuneBoxSlotType.Gold);
            }

            for (int i = pickCount - 1; i >= 0; i--)
            {
                int orderIndex = (pickNum + i) % _maxSlotcount;
                _pickSlotIdx.Add(targetOrder[orderIndex]);
            }

            return;
        }

        //Debug.Log(type + " : " + actualCount);
        //for (int i = 0; i < slotList.Count; ++i)
        //{
        //    Debug.Log(i + ": " + slotList[i].Name);
        //}

        for (int i = 0; i < _maxSlotcount; i++)
        {
            int orderIndex = (pickNum + i) % _maxSlotcount;
            int slotIndex = targetOrder[orderIndex];

            slots[slotIndex].SetSlot(slotList[i]);
        }

        // 당첨 슬롯 세팅
        for (int i = actualCount - 1; i >= 0; i--)
        {
            int orderIndex = (pickNum + i) % _maxSlotcount;
            _pickSlotIdx.Add(targetOrder[orderIndex]);
        }

    }


    [Button("열기")]
    public override void OpenUIInternal()
    {
        base.OpenUIInternal();

        SoundManager.Instance.PlaySfx(SfxName.Sfx_FortuneBox, idx: 2);

        _pickButton.SetInteractable(false);
        _pickButton.gameObject.SetActive(false);
        _pickButton.gameObject.transform.localScale = Vector3.zero;

        _skipButton.SetInteractable(false);
        _skipButton.gameObject.SetActive(false);
        _skipButton.gameObject.transform.localScale = Vector3.zero;

        StartCoroutine(ReadyRoutine());

    }

    [Button("기본")]
    void StartDefaultFocus()
    {
        PlayFocus(FocusRoutine(_defaultOrder, 1, 5));
    }

    [Button("빙글빙글 3")]
    void StartSpiralFocus()
    {
        PlayFocus(FocusRoutine(_spiralOrder, 1, 2));
    }

    [Button("지그재그 5")]
    void StartZigzagFocus()
    {
        PlayFocus(FocusRoutine(_zigzagOrder, 1, 5));
    }


    // 시작

    IEnumerator ReadyRoutine()
    {
        yield return new WaitForSecondsRealtime(DefaultPopupDuration);

        yield return PlayFocus(GroupFocusRoutine(_diagonalOrder, 3));

        _pickButton.gameObject.SetActive(true);
        _pickButton.transform.DOScale(1, 0.5f).SetUpdate(true).SetEase(Ease.OutBounce).OnComplete(
            () =>
            {
                _pickButton.SetInteractable(true);
                _pickButton._buttonAction += OnClickPickButton;
            });

    }

    public void OnClickPickButton()
    {
        _pickButton.transform.DOScale(0f, 0.2f).SetUpdate(true).SetEase(Ease.InCirc);


        // 뽑기 진행
        switch (_type)
        {
            case WaveClearRewardType.Fortune_Skill_1:
                _nowFocusRoutine =
                StartCoroutine(
                    RollRoutine(
                        PlayFocus(FocusRoutine(_defaultOrder, 1, 3))
                        )
                    );

                break;

            case WaveClearRewardType.Fortune_Skill_3:
                _nowFocusRoutine =
                StartCoroutine(
                    RollRoutine(
                        PlayFocus(FocusRoutine(_spiralOrder, 1, 3))
                        )
                    );
                break;

            case WaveClearRewardType.Fortune_Skill_5:
                _nowFocusRoutine =
                StartCoroutine(
                    RollRoutine(
                        PlayFocus(FocusRoutine(_zigzagOrder, 1, 3))
                        )
                    );

                break;
        }


        // 스킵버튼 보여주기
        StartCoroutine(ShowSkipButtonRoutine());

    }


    IEnumerator RollRoutine(Coroutine focusRoutine)
    {
        yield return focusRoutine;

        int[] targetOrder = _defaultOrder;

        switch (_type)
        {
            case WaveClearRewardType.Fortune_Skill_3:
                targetOrder = _spiralOrder;
                break;

            case WaveClearRewardType.Fortune_Skill_5:
                targetOrder = _zigzagOrder;
                break;
        }

        _nowFocusRoutine = StartCoroutine(ToTargetIndexRoutine(targetOrder));
    }


    IEnumerator ShowSkipButtonRoutine()
    {
        yield return _skipWaitTime;

        _skipButton.gameObject.SetActive(true);
        _skipButton.transform.DOScale(1, 0.5f).SetUpdate(true).SetEase(Ease.OutBounce).OnComplete(
            () =>
            {
                _skipButton.SetInteractable(true);
                _skipButton._buttonAction += OnClickSkipButton;
            });
    }

    public void OnClickSkipButton()
    {
        _skipButton.transform.DOScale(0f, 0.2f).SetUpdate(true).SetEase(Ease.InCirc).OnComplete(ShowBackButton);

        // 스킵하고 결과 바로 보여주기
        if (_nowFocusRoutine != null) StopCoroutine(_nowFocusRoutine);

        for (int i = 0; i < _pickSlotIdx.Count; i++)
        {
            int idx = _pickSlotIdx[i];
            if (idx >= slots.Length) continue;

            slots[idx].SetFocus(true);
        }

        SoundManager.Instance.PlaySfx(SfxName.Sfx_FortuneBox, idx: 1);
    }


    public void ShowBackButton()
    {
        _pickButton.gameObject.SetActive(false);
        _skipButton.gameObject.SetActive(false);
        _backButton.gameObject.SetActive(true);
    }

    // 창 닫힐 때 스킬 주기
    public override Tween CloseUIInternal()
    {
        for (int i = 0; i < _pickSlotIdx.Count; i++)
        {
            int slotIndex = _pickSlotIdx[i];
            FortuneBoxSlot slot = slots[slotIndex];

            if (slot.Skill == null) continue;

            switch (slot.SlotType)
            {
                case FortuneBoxSlotType.Skill:
                    StageManager.Instance.SkillSystem.TryAcquireSkill(slot.Skill.Id);
                    break;
                
                case FortuneBoxSlotType.Food:
                    PlayerManager.Instance.StagePlayer.Condition[StatType.Health].Add(_healAmount);
                    break;
                
                case FortuneBoxSlotType.Gold:
                    PlayerManager.Instance.StagePlayer.AddGold(_goldAmount);
                    break;
            }
        }

        for (int i = 0; i < _maxSlotcount; i++)
        {
            slots[i].SetFocus(false);
        }

        return base.CloseUIInternal();
    }


    #region FocusRoutine
    Coroutine PlayFocus(IEnumerator _routine)
    {
        if (_nowFocusRoutine != null)
        {
            StopCoroutine(_nowFocusRoutine);
        }

        _nowFocusRoutine = StartCoroutine(_routine);

        return _nowFocusRoutine;
    }

    IEnumerator FocusRoutine(int[] order, int focusTargetCount, int round)
    {
        int count = slots.Length;

        for (int i = 0; i < count; i++)
        {
            slots[i].SetFocus(false);
        }

        for (int i = 0; i < round; i++)
        {
            for (int j = 0; j < order.Length; j++)
            {
                int slotIndex = order[j];

                SoundManager.Instance.PlaySfx(SfxName.Sfx_FortuneBox, idx: 0);
                slots[slotIndex].SetFocusFade();

                yield return _intervalWait;
            }
        }

        _nowFocusRoutine = null;
    }

    IEnumerator ToTargetIndexRoutine(int[] order)
    {
        int targetIdx = _pickSlotIdx.Count - 1;
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(_stepInterval * 2f);

        for (int j = 0; j < order.Length && targetIdx >= 0; j++)
        {
            int slotIndex = order[j];

            if (_pickSlotIdx[targetIdx] == slotIndex)
            {
                SoundManager.Instance.PlaySfx(SfxName.Sfx_FortuneBox, idx: 1);
                slots[slotIndex].SetFocus(true);
                targetIdx--;
            }
            else
            {
                SoundManager.Instance.PlaySfx(SfxName.Sfx_FortuneBox, idx: 0);
                slots[slotIndex].SetFocusFade();
            }

            yield return wait;
        }

        ShowBackButton();
        _nowFocusRoutine = null;
    }

    // 그룹으로 되어있는 애들
    IEnumerator GroupFocusRoutine(int[][] groups, int round)
    {
        int count = slots.Length;

        for (int i = 0; i < count; i++)
        {
            slots[i].SetFocus(false);
        }

        for (int r = 0; r < round; r++)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                int[] group = groups[i];

                for (int k = 0; k < group.Length; k++)
                {
                    int idx = group[k];
                    slots[idx].SetFocusFade();
                }

                yield return _intervalWait;
            }
        }

        _nowFocusRoutine = null;
    }


    #endregion

}