using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LightingParticle : ParticlePoolObject
{
    [SerializeField] private float _moveDuration = 0.3f;
    [SerializeField] private float _rangeOffset = 0.2f;

    private TrailRenderer _trail;
    private Coroutine _coroutine;
    private WaitForEndOfFrame _delay = new();

    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
    }

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();
        DOTween.Kill(transform);
        _coroutine = StartCoroutine(DelayedStart());
    }

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        DOTween.Kill(transform);
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return _delay;

        Vector3 targetPos = transform.position;

        Camera camera = Camera.main;
        float startY = camera.transform.position.y + camera.orthographicSize;

        transform.position = new Vector2(targetPos.x, startY);

        _trail.enabled = false;
        _trail.Clear();
        _trail.enabled = true;

        Sequence seq = DOTween.Sequence().SetTarget(transform);
        float duration = _moveDuration / 3;

        seq.Append(transform.DOMove(
            new Vector2(
                targetPos.x + Random.Range(-_rangeOffset, _rangeOffset),
                Mathf.Lerp(startY, targetPos.y, 1f / 3f)
            ),
            duration
        )).SetEase(Ease.Linear);

        seq.Append(transform.DOMove(
            new Vector2(
                targetPos.x + Random.Range(-_rangeOffset, _rangeOffset),
                Mathf.Lerp(startY, targetPos.y, 2f / 3f)
            ),
            duration
        )).SetEase(Ease.Linear);

        seq.Append(transform.DOMove(
            targetPos,
            duration
        )).SetEase(Ease.Linear);

        seq.OnComplete(() => CommonPoolManager.Instance.Spawn(CommonPoolIndex.Particle_Explosion, transform.position));
    }
}
