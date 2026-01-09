using System.Collections;
using UnityEngine;

public class BossRushSkill : MonoBehaviour
{
    [Header("Detect")]
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private Transform target;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashDuration = 0.3f;

    [Header("Warning")]
    [SerializeField] private GameObject rushWarning;
    [SerializeField] private float warningTime = 1f;
    private Rigidbody2D rb;
    private bool isDashing;
    private bool isUsingSkill;
    public bool IsUsingSkill => isUsingSkill;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rushWarning.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isUsingSkill || target == null)
            return;

        float dist = Vector2.Distance(transform.position, target.position);

        // 7f 밖에서 감지
        if (dist <= detectRange)
        {
            Debug.Log(
               $"[BossRush] Detect Player | dist={dist:F2}, detectRange={detectRange}"
           );
            StartCoroutine(DashRoutine());
        }
    }
    private void Start()
    {
        if (target == null && PlayerManager.Instance != null && PlayerManager.Instance.StagePlayer != null)
        {
            target = PlayerManager.Instance.StagePlayer.transform;
            Debug.Log($"[BossRush] Target assigned from PlayerManager: {target.name}");
        }
    }
    private IEnumerator DashRoutine()
    {
        isUsingSkill = true;

        Vector2 startPos = rb.position;

        // 방향 고정
        Vector2 dashDir = ((Vector2)target.position - rb.position).normalized;

        // "감지된 거리만큼" 돌진하기 위해, 시작 시점 거리 저장
        float targetDistance = Vector2.Distance(rb.position, (Vector2)target.position);
        Debug.Log(
           $"[BossRush] Warning Start | targetDistance={targetDistance:F2}"
       );
        // 경고 표시
        rushWarning.SetActive(true);
        yield return new WaitForSeconds(warningTime);

        Debug.Log(
         $"[BossRush] Dash Start | speed={dashSpeed}, dir={dashDir}"
     );
        // 돌진
        isDashing = true;

        // startPos에서 targetDistance만큼 이동할 때까지 돌진
        while (Vector2.Distance(startPos, rb.position) < targetDistance)
        {
            rb.velocity = dashDir * dashSpeed;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
        rushWarning.SetActive(false);
        Debug.Log(
           $"[BossRush] Dash End | movedDistance={Vector2.Distance(startPos, rb.position):F2}"
       );

        isUsingSkill = false;
    }
}
