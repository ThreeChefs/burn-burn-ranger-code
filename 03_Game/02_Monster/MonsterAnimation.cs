using DG.Tweening;
using UnityEngine;

public class DOTweenBreathIdle : MonoBehaviour
{
    [Header("Target (비우면 자기 자신)")]
    [SerializeField] private Transform target;

    [Header("Breath (자연스러운 범위 추천)")]
    [SerializeField] private float scaleY = 1.1f;   // 1.03 ~ 1.08
    [SerializeField] private float scaleX = 0.9f;   // 0.97 ~ 0.995

    [Header("Motion (선택)")]
    [SerializeField] private bool useMove = false;   // 자연스러움 원하면 보통 false 추천
    [SerializeField] private float moveY = 0.05f;    // 0.01 ~ 0.04

    [Header("Timing")]
    [SerializeField] private float halfDuration = 0.6f; // 들숨/날숨 각각 시간 (총 2*halfDuration)
    [SerializeField] private float pause = 0.0f;        // 숨 멈춤(0이면 연속)

    [Header("Option")]
    [SerializeField] private bool randomDelay = true;

    private Vector3 baseScale;
    private Vector3 basePos;
    private Sequence seq;

    private void Awake()
    {
        if (target == null) target = transform;
        CaptureBase();
    }

    private void OnEnable()
    {
        Play();
    }

    private void OnDisable()
    {
        seq?.Kill();
        seq = null;

        // 풀링/비활성화 안정 원복
        if (target != null)
        {
            target.localScale = baseScale;
            if (useMove) target.localPosition = basePos;
        }
    }

    private void CaptureBase()
    {
        baseScale = target.localScale;
        basePos = target.localPosition;
    }

    public void Play()
    {
        // ✅ 런타임 기준 재캡처 (끌림 방지)
        CaptureBase();

        seq?.Kill();
        seq = DOTween.Sequence();

        if (randomDelay)
            seq.AppendInterval(Random.Range(0f, 0.5f));

        Vector3 inhaleScale = new Vector3(baseScale.x * scaleX, baseScale.y * scaleY, baseScale.z);

        // 들숨
        seq.Append(target.DOScale(inhaleScale, halfDuration).SetEase(Ease.InOutSine));
        if (useMove) seq.Join(target.DOLocalMoveY(basePos.y + moveY, halfDuration).SetEase(Ease.InOutSine));

        if (pause > 0f) seq.AppendInterval(pause);

        // 날숨(원복)
        seq.Append(target.DOScale(baseScale, halfDuration).SetEase(Ease.InOutSine));
        if (useMove) seq.Join(target.DOLocalMoveY(basePos.y, halfDuration).SetEase(Ease.InOutSine));

        if (pause > 0f) seq.AppendInterval(pause);

        seq.SetLoops(-1, LoopType.Restart);
    }
}