using UnityEngine;
/// <summary>
/// 젬 수집
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class GemCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GemItem gem))
        {
            gem.StartMagnet(transform);
        }
    }
}
