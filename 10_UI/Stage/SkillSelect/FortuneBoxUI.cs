
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class FortuneBoxUI : PopupUI
{

    [SerializeField] FortuneBoxSlot[] slots;

    [Header("Focus FX")]
    [SerializeField] float _stepInterval = 0.05f; 
    [SerializeField] int _rounds = 3;             

    Coroutine _nowFocusRoutine;

    WaitForSecondsRealtime _intervalWait;

    protected override void AwakeInternal()
    {
        base.AwakeInternal();
        _intervalWait = new WaitForSecondsRealtime(_stepInterval);
    }

    [Button("행운상자 UI 테스트")]
    public override void OpenUIInternal()
    {
        base.OpenUIInternal();
        PlayFocusCoroutine_Test();
    }

    void PlayFocusCoroutine_Test()
    {
        if (slots == null || slots.Length == 0) return;

        if (_nowFocusRoutine != null)
        {
            StopCoroutine(_nowFocusRoutine);
            _nowFocusRoutine = null;
        }

        _nowFocusRoutine = StartCoroutine(FocusLoopRoutine());
    }

    IEnumerator FocusLoopRoutine()
    {
        int count = slots.Length;

        for (int i = 0; i < count; i++)
            slots[i].SetFocus(false);

        int prev = -1;
        int totalSteps = count * _rounds;


        for (int step = 0; step < totalSteps; step++)
        {
            int idx = step % count;

            if (prev >= 0) slots[prev].SetFocus(false);
            slots[idx].SetFocus(true);
            prev = idx;

            yield return _intervalWait;
        }

        if (prev >= 0)
        {
            slots[prev].SetFocus(false);
        }

        _nowFocusRoutine = null;
    }


}
