using System.Collections;
using UnityEngine;

public class BossNormalPattern : BossPatternBase
{


    [Header("Telegraph (Ground Warning)")]
    [SerializeField] private GameObject warningPrefab;   // 장판(텔레그래프) 프리팹
    [SerializeField] private float range = 3f;           // 최종 반경(월드 유닛 기준)
    [SerializeField] private float warnTime = 0.8f;      // 알파/스케일 키우는 시간
    [SerializeField] private float keepTime = 0.15f;     // 완성 후 잠깐 유지(타격 직전 느낌만)
    [SerializeField] private float prepareTime = 3f;

    [Header("Visual")]
    [SerializeField] private float startAlpha = 0f;
    [SerializeField] private float endAlpha = 0.8f;
    [SerializeField] private float startScaleMul = 0.2f; // 시작 스케일 비율
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private LayerMask playerLayer; // StagePlayer 레이어 지정
    protected override bool CanRun()
    {

        bool canRun = warningPrefab != null && boss != null && boss.Target != null;




        return canRun;

    }

    protected override IEnumerator Execute()
    {


        // 1) 보스 준비: 3초간 가만히
        boss.SetPatternLock(true); // 보스 움직임/다른 패턴 정지

        yield return new WaitForSeconds(prepareTime);

        // 2) 준비 끝난 시점의 플레이어 위치를 저장(찍을 위치 확정)
        Transform target = boss.Target;
        if (target == null)
        {
            boss.SetPatternLock(false);
            yield break;
        }

        Vector3 slamPos = target.position;
        slamPos.z = 0f;
        boss.transform.position = slamPos;

        Collider2D hit = Physics2D.OverlapCircle(slamPos, range, playerLayer);

        if (hit != null)
        {
            // StagePlayer가 IDamageable을 구현했으면 이게 제일 깔끔
            if (hit.TryGetComponent<IDamageable>(out var dmg))
            {
                float damage = boss.Attack;

                dmg.TakeDamage(boss.Attack);
            }
            // 아니면 StagePlayer에 맞는 함수/스탯 처리로 바꿔도 됨
            else if (hit.TryGetComponent<StagePlayer>(out var player))
            {
                player.TakeDamage(boss.Attack); // 네 프로젝트에 있는 방식으로 수정
            }


            // 3) 장판 생성(고정)
            GameObject go = Instantiate(warningPrefab, slamPos, Quaternion.identity);
            go.transform.SetParent(null, true);

            SpriteRenderer sr = go.GetComponentInChildren<SpriteRenderer>(true);
            if (sr == null)
            {

                Destroy(go);
                boss.SetPatternLock(false);
                yield break;
            }
            float radius = 3f;
            float targetDiameter = radius * 2f; // 6f

            // 현재 스프라이트의 월드 크기
            Vector2 spriteWorldSize = sr.bounds.size;

            // 원형 기준이므로 큰 축 기준으로 계산
            float currentDiameter = Mathf.Max(spriteWorldSize.x, spriteWorldSize.y);

            // 몇 배 키워야 하는지
            float scaleMul = targetDiameter / currentDiameter;

            // 최종 스케일 적용
            go.transform.localScale *= scaleMul;
            // 초기화
            Color c = sr.color;
            c.a = startAlpha;
            sr.color = c;




            // 4) 2초 동안 장판 퍼짐(연출)
            float t = 0f;
            while (t < warnTime)
            {
                t += Time.deltaTime;
                float u = Mathf.Clamp01(t / warnTime);
                float k = ease.Evaluate(u);


                c.a = Mathf.Lerp(startAlpha, endAlpha, k);
                sr.color = c;

                yield return null;
            }

            // 5) 장판 완성 순간: 보스 순간이동(찍는 연출)
            boss.transform.position = slamPos;


            // (선택) 쿵 찍은 뒤에도 계속 멈춰있게 하려면 여기서 추가 대기 가능
            // yield return new WaitForSeconds(0.3f);

            // 6) 보스 정지 해제(이제 다시 움직이게)
            boss.SetPatternLock(false);

            // 7) 마무리
            yield return new WaitForSeconds(keepTime);
            Destroy(go);


        }
    }
}
