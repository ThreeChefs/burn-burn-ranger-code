using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float damagePerSecond = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player))
        {
            return;
        }
        player.TakeDamage(damagePerSecond * Time.deltaTime);
    }
}
