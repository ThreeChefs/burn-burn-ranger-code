using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDiePattern : BossPatternBase
{
    [Header("기믹 실행 시작")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float arriveDistance = 0.2f;
    [SerializeField] private float maxMoveTime = 3f;
    [SerializeField] private bool snapOnArrive = true;

    private Rigidbody2D _rb;

    [Header("기믹들")]
    [SerializeField] private WavePattern wavePattern;
    [SerializeField] private float prePareTime = 1f;
    [Header("패턴 시퀀스")]
    [SerializeField] private bool useAABB = true; // 테스트용 토글

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override bool CanRun()
    {
        if (boss == null)
        {
            return false;
        }
        if (boss.IsDead)
        {
            return false;
        }

        return true;
    }

    protected override IEnumerator Execute()
    {

        if (boss == null || boss.IsDead)
        {

            yield break;
        }

        boss.SetPatternLock(true);

        yield return MoveToCenterRoutine();

        if (boss == null || boss.IsDead)
        {

            yield break;
        }
        if (prePareTime > 0f)
        {

            yield return new WaitForSeconds(prePareTime);
        }

        var player = PlayerManager.Instance.StagePlayer;
        if (player != null)
        {

            wavePattern.BindPlayer(player.transform);
        }
        List<float> seq = new List<float> { 0f, 45f, 90f, 135f };


        yield return wavePattern.RunSequence(Vector3.zero, seq);
        boss.SetPatternLock(false);
    }

    private IEnumerator MoveToCenterRoutine()
    {
        Vector3 target = Vector3.zero;
        float elapsed = 0f;

        // 다른 이동 로직의 영향을 막기 위해 속도를 0으로 초기화
        if (_rb != null)
            _rb.velocity = Vector2.zero;

        // 제한 시간(maxMoveTime) 동안 중앙으로 이동 시도
        while (elapsed < maxMoveTime)
        {
            // 이동 중 보스가 죽었으면 즉시 중단
            if (boss == null || boss.IsDead)
                yield break;

            Vector3 cur = boss.transform.position;

            // 중앙과의 거리 계산
            float dist = Vector2.Distance(cur, target);

            // 도착 판정 거리 이내면 이동 종료
            if (dist <= arriveDistance)
                break;

            if (_rb != null)
            {
                // Rigidbody2D가 있으면 물리 이동
                Vector2 next = Vector2.MoveTowards(
                    _rb.position,
                    target,
                    moveSpeed * Time.fixedDeltaTime
                );

                _rb.MovePosition(next);

                // FixedUpdate 타이밍에 맞춰 대기
                yield return new WaitForFixedUpdate();
            }
            else
            {
                // Rigidbody가 없으면 Transform 기반 이동
                boss.transform.position = Vector3.MoveTowards(
                    cur,
                    target,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            elapsed += Time.deltaTime;
        }

        // 도착 시 위치를 정확히 중앙으로 스냅
        if (snapOnArrive && boss != null)
        {
            if (_rb != null)
                _rb.position = target;
            else
                boss.transform.position = target;
        }
    }

}

