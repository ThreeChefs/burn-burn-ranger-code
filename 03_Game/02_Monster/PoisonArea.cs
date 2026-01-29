using UnityEngine;

public class PoisonArea : BaseProjectile
{
    [SerializeField] private float defaultDuration = 3f;

    private float _dps;
    private float _disableTime;

    public void Init(float dps, float durationOverride = -1f)
    {
        _dps = dps;
        float life = durationOverride > 0f ? durationOverride : defaultDuration;
        _disableTime = Time.time + life;
    }

    protected override void OnEnableInternal()
    {
        base.OnEnableInternal();
        // Init이 늦게 호출되더라도 1차 안전장치
        _disableTime = Time.time + defaultDuration;
    }

    protected override void OnDisableInternal()
    {
        base.OnDisableInternal();
        _dps = 0f;
    }

    protected override void Update()
    {
        if (Time.time >= _disableTime)
            gameObject.SetActive(false); // ✅ 풀 반납
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;

        if (player == null) return;

        player.TakeDamage(_dps * Time.deltaTime);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 장판은 BaseProjectile의 즉시 히트 로직을 타면 안 됨
    }
}