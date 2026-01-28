using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private Image _fillImg;
    
    private Coroutine _routine;
    private Tween _fullLoop;

    Color _originFillColor;

    bool _levelup;
    float _nextValue = 0f;

    
    private void Awake()
    {
        _slider.value = 0;
        _originFillColor = _fillImg.color;
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

        if (_levelup)
        {
            _routine = StartCoroutine(LerpSliderValue(startValue, 1, _duration));
            _nextValue = targetValue;
            FullLoopTween();
        }
        else
        {
            _routine = StartCoroutine(LerpSliderValue(startValue, targetValue, _duration));
        }
    }

    public void SetNextExpValue()
    {
        _levelup = false;
        _fillImg.DOKill();

        _fillImg.color = _originFillColor;

        float targetValue = PlayerManager.Instance.StagePlayer.StageLevel.CurrentExp / PlayerManager.Instance.StagePlayer.StageLevel.RequiredExp;
        float startValue = _slider.value;
        _routine = StartCoroutine(LerpSliderValue(startValue, targetValue, _duration));
        
    }

    void FullLoopTween()
    {
        float hueCycleTime = 1f;

        float h;
        float s;
        float v;

        Color.RGBToHSV(_fillImg.color, out h, out s, out v);

        _fillImg.DOKill();

        DOTween.To(
            () => h,
            x =>
            {
                h = x;
                float wrappedH = h % 1f;
                _fillImg.color = Color.HSVToRGB(wrappedH, s, v);
            },
            h + 1f,                // 한 바퀴 증가
            hueCycleTime
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental)
        .SetUpdate(true)
        .SetTarget(_fillImg);
    }

    void UpdateLevel(int level)
    {
        _levelup = true;
        _levelText.text = level.ToString();
    }

    IEnumerator LerpSliderValue(float start, float end, float duration)
    {
        float alpha = 0f;

        while (alpha < duration)
        {
            alpha += Time.unscaledDeltaTime;
            float t = alpha / duration;
            _slider.value = Mathf.Lerp(start, end, t);
            yield return null;
        }

        _slider.value = end;
        _routine = null;
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance.StagePlayer == null) return;

        PlayerManager.Instance.StagePlayer.StageLevel.OnExpChanged -= UpdateValue;
        PlayerManager.Instance.StagePlayer.StageLevel.OnLevelChanged -= UpdateLevel;
    }

}
