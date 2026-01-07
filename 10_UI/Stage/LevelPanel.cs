using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 0.3f;
    private Coroutine _routine;

    private void Awake()
    {
        _slider.value = 0;
    }

    private void Start()
    {
        PlayerManager.Instance.StagePlayer.StageLevel.OnExpChanged += UpdateValue;
        PlayerManager.Instance.StagePlayer.StageLevel.OnLevelChanged += UpdateLevel;
        _levelText.text = PlayerManager.Instance.StagePlayer.StageLevel.Level.ToString();
    }

    void UpdateValue(float targetValue)
    {
        if (_routine != null)
            StopCoroutine(_routine);

        float startValue = _slider.value;
        _routine = StartCoroutine(LerpSliderValue(startValue, targetValue, _duration));
    }

    void UpdateLevel(int level)
    {
        _levelText.text = level.ToString();
    }

    IEnumerator LerpSliderValue(float start, float end, float duration)
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
