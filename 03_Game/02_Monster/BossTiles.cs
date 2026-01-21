using UnityEngine;

public class BossTiles : MonoBehaviour
{

    [SerializeField] private float damage = 10f;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player)) return;

        if (other.TryGetComponent<IDamageable>(out var dmg))
            dmg.TakeDamage(damage);
    }
}
