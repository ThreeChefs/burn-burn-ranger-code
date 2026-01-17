using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    private LaserPattern owner;
    public void Bind(LaserPattern laser) => owner = laser;

    [Header("Impact Effect")]
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private float vfxTick = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool debugLog = true;

    private float nextVfxTime;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner == null)
        {
            return;
        }
        if (!other.TryGetComponent<StagePlayer>(out var player))
        {
            return;
        }

        SpawnFlash(other);

        // Enter에서 한 번 쐈으니, 다음 VFX는 tick 뒤로 미룸(중복 방지)
        nextVfxTime = Time.time + vfxTick;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (owner == null)
        {
            return;
        }
        if (!other.TryGetComponent<StagePlayer>(out var player))
        {
            return;
        }
        owner.ApplyTickDamage(other);
        if (Time.time >= nextVfxTime)
        {
            SpawnFlash(other);
            nextVfxTime = Time.time + vfxTick;
        }
    }

    private void SpawnFlash(Collider2D collider)
    {


        Vector2 hitPos = collider.ClosestPoint(transform.position);
        Vector2 direction = transform.right;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;



        Instantiate(
            impactEffectPrefab,
            hitPos,
            Quaternion.Euler(0f, 0f, angle)
        );
    }
}
