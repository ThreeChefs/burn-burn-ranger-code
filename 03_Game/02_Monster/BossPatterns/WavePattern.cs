using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePattern : MonoBehaviour
{
    [Header("장판")]
    [SerializeField] private Transform dangerCircle;
    [SerializeField] private Transform safeArea;
    [SerializeField] private Transform damageArea;
    [Header("Area Settings")]
    [SerializeField] private float areaRadius = 6f;
    [SerializeField] private float safeAreaWidth = 3f;

    [Header("Time")]
    [SerializeField] private float warningTime = 0.7f;
    [SerializeField] private float activeTime = 0.3f;
    [SerializeField] private float stepInterval = 0.1f;

    [Header("데미지")]
    [SerializeField] private float damage = 30f;
    [SerializeField] private bool instantCheckOnce = true;

    [Header("디버그")]
    [SerializeField] private bool debugGizmos = true;

    private WaitForSeconds _waitWarning;
    private WaitForSeconds _waitInterval;
    private Transform _player;
    private void Awake()
    {
        if (dangerCircle != null)
        {
            dangerCircle.gameObject.SetActive(false);
        }
        if (safeArea != null)
        {
            safeArea.gameObject.SetActive(false);
        }
        if (damageArea != null)
        {
            damageArea.gameObject.SetActive(false);
        }
        _waitWarning = new WaitForSeconds(warningTime);
        _waitInterval = new WaitForSeconds(stepInterval);
    }

    public void BindPlayer(Transform player)
    {
        _player = player;
    }

    public IEnumerator RunSequence(Vector3 centerPos, List<float> angleDegSequence)
    {
        dangerCircle.position = centerPos;
        safeArea.position = centerPos;

        dangerCircle.gameObject.SetActive(true);

        float diameter = areaRadius * 3.5f;
        Vector3 stripeScale = new Vector3(diameter, safeAreaWidth * 5f, 5f);
        safeArea.localScale = stripeScale;

        if (damageArea != null)
        {
            damageArea.position = centerPos;
            damageArea.localScale = stripeScale;

        }
        safeArea.gameObject.SetActive(true);

        for (int i = 0; i < angleDegSequence.Count; i++)
        {
            float angle = angleDegSequence[i];
            safeArea.rotation = Quaternion.Euler(0f, 0f, angle);
            yield return _waitWarning;
            yield return _waitInterval;
        }
        safeArea.gameObject.SetActive(false);
        for (int i = 0; i < angleDegSequence.Count; i++)
        {
            float angle = angleDegSequence[i];

            if (damageArea != null)
            {
                damageArea.rotation = Quaternion.Euler(0f, 0f, angle);
                damageArea.gameObject.SetActive(true);
            }
            if (instantCheckOnce)
            {
                HitCheck(centerPos, angle);
                yield return _waitWarning;
            }
            else
            {
                yield return DoContinuousHitCheck(centerPos, angle, warningTime);
            }
            if (damageArea != null)
            {
                damageArea.gameObject.SetActive(false);
            }
            yield return _waitInterval;
        }
        dangerCircle.gameObject.SetActive(false);
        if (damageArea != null) damageArea.gameObject.SetActive(false);
        safeArea.gameObject.SetActive(false);
    }

    private void HitCheck(Vector3 centerPos, float angleDeg)  // 데미지 판정
    {
        if (_player == null)
        {
            return;
        }
        if (IsInsideArena(_player.position, centerPos) && !IsInSafeStripe(_player.position, centerPos, angleDeg))
        {
            ApplyDamageToPlayer(_player, damage);
        }
    }
    private IEnumerator DoContinuousHitCheck(Vector3 centerPos, float angleDeg, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            if (_player != null)
            {
                if (IsInsideArena(_player.position, centerPos) && !IsInSafeStripe(_player.position, centerPos, angleDeg))
                {
                    ApplyDamageToPlayer(_player, damage);

                    break;
                }
            }

            t += Time.deltaTime;
            yield return null;
        }
    }
    private bool IsInsideArena(Vector3 worldPos, Vector3 centerPos)
    {
        Vector2 v = worldPos - centerPos;
        return v.magnitude <= areaRadius;
    }
    private bool IsInSafeStripe(Vector3 worldPos, Vector3 centerPos, float angleDeg)
    {
        Vector2 p = worldPos - centerPos;

        // 띠의 "길이 방향" 단위벡터 (angle 기준)
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector2 stripeDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        // 띠의 "폭 방향"(법선) = 길이방향을 90도 회전
        Vector2 normal = new Vector2(-stripeDir.y, stripeDir.x);


        float distToCenterLine = Mathf.Abs(Vector2.Dot(p, normal));
        return distToCenterLine <= safeAreaWidth;
    }
    private void ApplyDamageToPlayer(Transform player, float dmg)
    {

        if (player.TryGetComponent<IDamageable>(out var d))
        {
            d.TakeDamage(dmg);
        }
        else
        {
            Debug.Log($"[AlbionStripeGimmick] Player hit! dmg={dmg}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }

}
