using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;

    private bool _hasHit;

    private void OnEnable()
    {
        _hasHit = false;
        Invoke(nameof(Disable), lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어만 피격
        if (_hasHit)
            return;

        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;

        _hasHit = true;
        // ⭐ 데미지 디버그

        player.TakeDamage(damage);

        Disable();
    }

    private void Disable()
    {
        CancelInvoke();
        Destroy(gameObject); // 풀링 쓰면 SetActive(false)
    }
}
