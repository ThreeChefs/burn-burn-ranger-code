using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

public class WarningSignUI : PopupUI
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

        SoundManager.Instance.PlaySfx(SfxName.Sfx_Warning);
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
