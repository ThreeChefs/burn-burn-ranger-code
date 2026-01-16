using System.Collections;
using UnityEngine;

public abstract class BossPatternBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private string patternName = "";
    [SerializeField] private int priority = 0;         // 높을수록 먼저 실행
    [SerializeField] private float patterncooldown = 0f;      // 실행 후 재사용 쿨다운
    [SerializeField] private bool runOnlyOnce = false; // 1회성(HP 기믹 등에 유용)


    protected BossController boss;

    public float _nextReadyTime;
    private bool _hasRunOnce;

    public string PatternName => string.IsNullOrWhiteSpace(patternName) ? GetType().Name : patternName;
    public int Priority => priority;

    public void Bind(BossController bossController)
    {
        boss = bossController;

    }

    public bool IsAvailable()
    {
        if (boss == null || boss.IsDead) return false;
        if (runOnlyOnce && _hasRunOnce) return false;
        if (Time.time < _nextReadyTime) return false;

        return CanRun();
    }

    /// <summary>패턴 실행 조건 (HP% 도달, 상태 조건 등) </summary>
    protected abstract bool CanRun();

    /// <summary>패턴 실제 로직</summary>
    protected abstract IEnumerator Execute();

    /// <summary>컨트롤러가 실행할 때 부르는 공용 루틴</summary>
    public IEnumerator Run()
    {

        yield return Execute();

        _hasRunOnce = true;
        _nextReadyTime = Time.time + patterncooldown;


    }

    /// <summary>강제 종료 시 정리(선택)</summary>
    public virtual void Cleanup()
    {
        // 경고 오브젝트 끄기, 히트박스 비활성, 소환 중단 등
    }
}
