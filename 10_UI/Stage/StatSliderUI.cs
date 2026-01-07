using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSliderUI : BaseUI
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 0.3f;
    
    private BaseStat _targetStat;
    private Coroutine _routine;
    
    public void Init(BaseStat stat)
    {
        if (stat == null) return;
        
        _targetStat = stat;
        _targetStat.OnCurValueChanged += UpdateValue;
    }
    
    void UpdateValue(float targetValue)
    {
        if (_routine != null)
            StopCoroutine(_routine);

        float startValue = _slider.value;
        _routine = StartCoroutine(LerpProgress(startValue, targetValue, _duration));
    }

    IEnumerator LerpProgress(float start, float end, float duration)
    {
        float alpha = 0f;

        while (alpha < duration)
        {
            alpha += Time.deltaTime;
            float t = alpha / duration;
            _slider.value = Mathf.Lerp(start, end, t);
            yield return null;
        }

        _slider.value = end;
        _routine = null;
    }

}
