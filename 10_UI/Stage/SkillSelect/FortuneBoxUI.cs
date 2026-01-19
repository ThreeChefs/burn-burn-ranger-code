using Sirenix.OdinInspector;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FortuneBoxUI : PopupUI
{
    [SerializeField] FortuneBoxSlot[] slots;

    [Header("슬롯 설정")]
    [SerializeField] int _columnCount = 4;

    [Header("Focus 연출")]
    [SerializeField] float _stepInterval = 0.05f;   // 포커스 속도
    [SerializeField] int _focusTargetCount = 1;     // 포커스 타겟

    // 빙글빙글 안, 밖, 안, 밖 으로 4x4 일때 
    // 이 순서로 5개일때 연속으로 있을 자리를 픽해가자
    int[] _spiralOrder = new int[]{ 0,1,2,3,7,11,15,14,13,12,8,4,5,6,10,9,
                                    5,6,10,14,13,12,8,4,0,1,2,3,7,11,15,
                                    14,13,12,8,4,0,1,2,3,7,11,10,9,5,6,
                                    10,9,5,1,2,3,7,11,15,14,13,12,8,4 };

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


    Coroutine _nowFocusRoutine;
    WaitForSecondsRealtime _intervalWait;

    protected override void AwakeInternal()
    {
        base.AwakeInternal();
        _intervalWait = new WaitForSecondsRealtime(_stepInterval);
    }

    public override void OpenUIInternal()
    {
        base.OpenUIInternal();

    }

    [Button("기본")]
    void StartDefaultFocus()
    {
        PlayFocus(FocusRoutine(_defaultOrder, 1, 5));
    }

    [Button("빙글빙글 3")]
    void StartSpiralFocus()
    {
        PlayFocus(FocusRoutine(_spiralOrder, 3, 2));
    }

    [Button("지그재그 5")]
    void StartZigzagFocus()
    {
        PlayFocus(FocusRoutine(_zigzagOrder, 5, 5));
    }


    [Button("대각선")]
    void StartDiagonalFocus()
    {
        PlayFocus(GroupFocusRoutine(_diagonalOrder, 3));
    }

    void PlayFocus(IEnumerator _routine)
    {
        if (_nowFocusRoutine != null)
        {
            StopCoroutine(_nowFocusRoutine);
        }

        _nowFocusRoutine = StartCoroutine(_routine);
    }

    IEnumerator FocusRoutine(int[] order, int focusTargetCount, int round)
    {
        int count = slots.Length;

        for (int i = 0; i < count; i++)
        {
            slots[i].SetFocus(false);
        }

        int[] prves = new int[focusTargetCount];

        for (int i = 0; i < focusTargetCount; i++)
        {
            prves[i] = -1;
        }

        for (int j = 0; j < round; j++)
        {
            for (int i = 0; i < order.Length; i++)
            {
                for (int k = 0; k < focusTargetCount; k++)
                {
                    int idx = prves[k];
                    if (idx >= 0)
                        slots[idx].SetFocus(false);
                    prves[k] = -1;
                }

                for (int k = 0; k < focusTargetCount; k++)
                {
                    int orderIndex = (i + k) % order.Length;
                    int slotIndex = order[orderIndex];

                    slots[slotIndex].SetFocus(true);
                    prves[k] = slotIndex;
                }

                yield return _intervalWait;
            }
        }

        for (int k = 0; k < focusTargetCount; k++)
        {
            int idx = prves[k];
            if (idx >= 0)
                slots[idx].SetFocus(false);
        }

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

        int[] prevGroup = null;

        for (int r = 0; r < round; r++)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                if (prevGroup != null)
                {
                    for (int k = 0; k < prevGroup.Length; k++)
                    {
                        int idx = prevGroup[k];
                        if (idx >= 0 && idx < count)
                            slots[idx].SetFocus(false);
                    }
                }

                int[] group = groups[i];
                for (int k = 0; k < group.Length; k++)
                {
                    int idx = group[k];
                    if (idx >= 0 && idx < count)
                        slots[idx].SetFocus(true);
                }

                prevGroup = group;

                yield return _intervalWait;
            }
        }

        if (prevGroup != null)
        {
            for (int k = 0; k < prevGroup.Length; k++)
            {
                int idx = prevGroup[k];
                if (idx >= 0 && idx < count) slots[idx].SetFocus(false);
            }
        }

        _nowFocusRoutine = null;
    }

}
