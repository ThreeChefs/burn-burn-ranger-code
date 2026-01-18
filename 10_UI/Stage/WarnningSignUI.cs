using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarnningSignUI : PopupUI
{
    [Title("WarnningSign Settings")]
    [SerializeField] TextMeshProUGUI _signText;
    [SerializeField] float _delayTime = 3.0f;
    
    WaitForSeconds _delayWait;
    Coroutine _coroutine;

    protected override void AwakeInternal()
    {
        base.AwakeInternal();

        _delayWait = new WaitForSeconds(_delayTime);
    }


    public override void OpenUIInternal()
    {
        base.OpenUIInternal();

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(DelayHide());
    }


    public void SetText(string text)
    {
        _signText.text = text;
    }

    IEnumerator DelayHide()
    {
        yield return _delayWait;

        CloseUI();

    }

}
