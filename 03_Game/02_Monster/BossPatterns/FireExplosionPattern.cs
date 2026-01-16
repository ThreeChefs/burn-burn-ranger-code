using System.Collections;
using UnityEngine;

public class FireExplosionPattern : BossPatternBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject warningCirclePrefab;
    [SerializeField] private GameObject fireWallPrefab;
    [SerializeField] private GameObject explosionPrefab;

    [Header("Warning Circle Settings")]
    [SerializeField] private float warningTime = 0.8f;
    [SerializeField] private float warningLifeTime = 0.5f;
    [SerializeField] private bool scaleWithTime = true;
    [Header("Settings")]
    [SerializeField] private int spawnCount = 4;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnOffsetRadius;

    [Header("Attack CalCulator")]
    [SerializeField] private float AttackMultiplier = 0.3f;
    [SerializeField] private float tickInterval = 0.2f;
    [SerializeField] private float lifeTime = 6f;
    protected override bool CanRun()
    {
        // 보스와 불덩이 프리팹이 있어야 실행 가능
        return boss != null && fireWallPrefab != null;
    }

    protected override IEnumerator Execute()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 snapshotPos = boss.Target != null ? boss.Target.position : boss.transform.position;
            if (warningCirclePrefab != null)
            {
                GameObject warning = Instantiate(warningCirclePrefab, snapshotPos, Quaternion.identity);
                yield return StartCoroutine(PlayWarningFill(warning, warningTime));

                Destroy(warning, warningTime);
            }
            else
            {
                // 경고 프리팹 없으면 그냥 잠깐 딜레이라도
                yield return new WaitForSeconds(warningTime);
            }
            if (explosionPrefab != null)
            {
                var vfx = Instantiate(explosionPrefab, snapshotPos, Quaternion.identity);
                Destroy(vfx, 2f);
            }

            GameObject fireBall = Instantiate(
            fireWallPrefab,
             snapshotPos,
             Quaternion.identity);
            var dot = fireBall.GetComponent<FireArea>();
            if (dot != null)
            {
                float damagePerTick = boss.Attack * AttackMultiplier;
                dot.SetDamage(damagePerTick, tickInterval);
            }
            Destroy(fireBall, lifeTime);
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator PlayWarningFill(GameObject warningObj, float duration)
    {
        // warningCirclePrefab 안에 SpriteRenderer가 있다고 가정
        // (만약 Image(UI)라면 Image.fillAmount로 바꾸면 됨)
        var sr = warningObj.GetComponentInChildren<SpriteRenderer>(true);
        if (sr == null)
            yield break;

        // 시작: 거의 투명
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        Vector3 startScale = warningObj.transform.localScale;
        Vector3 endScale = scaleWithTime ? startScale * 1.1f : startScale;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            // 알파가 차오르듯 증가 (원하면 색도 바꿀 수 있음)
            c.a = Mathf.Lerp(0.1f, 0.8f, p);
            sr.color = c;

            if (scaleWithTime)
                warningObj.transform.localScale = Vector3.Lerp(startScale, endScale, p);

            yield return null;
        }
    }
}
