using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    [Header("Fire Area")]
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickInterval = 0.5f;

    private readonly Dictionary<int, float> nextTick = new();

    public void SetDamage(float dmgPerTick, float interval)
    {
        tickInterval = interval;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var player))
            return;
        {
            int id = other.gameObject.GetInstanceID();
            float now = Time.time;

            if (nextTick.TryGetValue(id, out float next) && now < next)
            {
                return;
            }
            nextTick[id] = now + tickInterval;
            player.TakeDamage(damagePerTick);
        }
    }
    private void OnDisable()
    {
        nextTick.Clear();
    }
}
