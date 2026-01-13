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
            yield return new WaitForSeconds(warningTime);
            yield return new WaitForSeconds(stepInterval);
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
                DoSingleHitCheck(centerPos, angle);
                yield return new WaitForSeconds(activeTime);
            }
            else
            {
                yield return DoContinuousHitCheck(centerPos, angle, activeTime);
            }
            if (damageArea != null)
            {
                damageArea.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(stepInterval);
        }
        dangerCircle.gameObject.SetActive(false);
        if (damageArea != null) damageArea.gameObject.SetActive(false);
        safeArea.gameObject.SetActive(false);
    }

    private void DoSingleHitCheck(Vector3 centerPos, float angleDeg)
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
                    // 연속 데미지면 틱 간격을 두는 게 보통이라 여기서 0.2f 같은 딜레이를 줘도 됨
                    // 지금은 간단히 한 번 맞추고 빠져나오게 하려면 break;
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

        // 플레이어 위치를 법선 방향으로 투영한 값의 절댓값이 halfWidth 이내면 안전
        float distToCenterLine = Mathf.Abs(Vector2.Dot(p, normal));
        return distToCenterLine <= safeAreaWidth;
    }
    private void ApplyDamageToPlayer(Transform player, float dmg)
    {
        // 너 프로젝트에 맞춰 연결하면 됨.
        // 예: StagePlayer or IDamageable
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
