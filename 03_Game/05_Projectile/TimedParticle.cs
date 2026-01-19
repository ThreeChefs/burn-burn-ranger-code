using UnityEngine;

/// <summary>
/// 특정 시간 동안 활성화하는 파티클
/// </summary>
public class TimedParticle : PoolObject
{
    [SerializeField] private float _particleTime = 0.2f;

    private ParticleSystem _particleSystem;
    private float _timer;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();
        _timer = 0f;
        _particleSystem.Play();
    }

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        _particleSystem.Stop();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _particleTime)
        {
            gameObject.SetActive(false);
            _timer = 0f;
        }
    }
}
