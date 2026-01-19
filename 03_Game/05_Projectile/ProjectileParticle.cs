using UnityEngine;

/// <summary>
/// 투사체용 이펙트
/// todo: 풀에 담기
/// </summary>
public class ProjectileParticle : PoolObject
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
            Destroy(gameObject);
            // todo: pool로 변경 후 disable
            _timer = 0f;
        }
    }
}
