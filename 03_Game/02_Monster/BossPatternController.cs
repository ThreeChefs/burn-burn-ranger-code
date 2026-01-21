using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossPatternController : MonoBehaviour    // 보스가 가진 패턴.cs들 (BossPatternBase를 상속받은 친구들) 을 수집하여 
{                                                     // 특정 조건에 도달하면 특정 패턴을 실행하는 스크립트. 
    [Header("Refs")]                                  // 패턴을 코루틴이나 어떻게 실행할건지 담당. 
    [SerializeField] private BossController boss;

    [Header("Settings")]
    [SerializeField] private float thinkInterval = 0.1f; // 보스 패턴 진입 딜레이시간 
    [SerializeField] private bool CollectPatternBase = true;




    public event Action<BossPatternBase> OnPatternStarted;
    public event Action<BossPatternBase> OnPatternFinished;

    private readonly List<BossPatternBase> _patterns = new();
    private Coroutine _running;
    private BossPatternBase _current;

    private float _nextThinkTime;

    public bool IsRunning => _running != null;

    public void Bind(BossController bossController)
    {
        boss = bossController;
        CollectPatterns();
        // 이미 붙어있는 패턴들에게 boss 바인딩
        for (int i = 0; i < _patterns.Count; i++)  // 캐싱한 _patterns 를 상속된 친구들 수집하여 패턴실행
            _patterns[i].Bind(boss);
    }

    private void Awake()
    {
        if (CollectPatternBase)
            CollectPatterns();
    }


    public void CollectPatterns()  //패턴 모아두는곳 
    {
        _patterns.Clear();


        GetComponentsInChildren(true, _patterns);
        _patterns.RemoveAll(p => p == null);




        _patterns.RemoveAll(p => p is BossDiePattern);


        if (boss != null)
        {
            for (int i = 0; i < _patterns.Count; i++)
                _patterns[i].Bind(boss);


        }


    }

    public void Tick()
    {
        if (boss == null || boss.IsDead) return;





        if (IsRunning) return;

        if (Time.unscaledTime < _nextThinkTime) return;
        _nextThinkTime = Time.unscaledTime + thinkInterval;

        var next = ChooseNextPattern();
        if (next != null)
            Play(next);
    }

    private BossPatternBase ChooseNextPattern()    // prioty로 우선순위를 정한이유: 노말패턴중에 중복으로 나오게하지않기위해 
    {                                              //1번 노말패턴이 사용되면, 우선순위를 나중에두어 다음패턴을 실행하기위함. 
                                                   // ex) (1번패턴:박치기) 박치기 박치기 박치기 박치기 박치기 박치기 박치기 방지
                                                   // 1) 가능한 패턴들만 모으기
        List<BossPatternBase> candidates = null;  //실행 가능한 패턴들 리스트

        for (int i = 0; i < _patterns.Count; i++)
        {
            var p = _patterns[i];
            if (p == null) continue;
            if (!p.IsAvailable()) continue;

            candidates ??= new List<BossPatternBase>();
            candidates.Add(p);
        }

        if (candidates == null || candidates.Count == 0)
            return null;


        int bestPrio = int.MinValue;
        for (int i = 0; i < candidates.Count; i++)
            bestPrio = Mathf.Max(bestPrio, candidates[i].Priority);


        var best = new List<BossPatternBase>();
        for (int i = 0; i < candidates.Count; i++)
            if (candidates[i].Priority == bestPrio)
                best.Add(candidates[i]);

        var chosen = best[UnityEngine.Random.Range(0, best.Count)];


        return chosen;
    }

    public void Play(BossPatternBase pattern)
    {
        if (pattern == null) return;
        if (IsRunning) return;

        _running = StartCoroutine(RunRoutine(pattern));
    }

    private IEnumerator RunRoutine(BossPatternBase pattern) //패턴을 코루틴으로 사용하여, 끝나면 패턴종료를 이벤트로 알린다.
    {
        _current = pattern;

        OnPatternStarted?.Invoke(pattern);

        yield return pattern.Run();

        OnPatternFinished?.Invoke(pattern);

        _current = null;
        _running = null;
    }

    public void CancelCurrent(bool invokeFinished = true)   // 사망, 다른패턴진입시 다른패턴 캔슬 
    {
        if (_running == null) return;

        StopCoroutine(_running);
        _running = null;

        if (_current != null)
        {
            _current.Cleanup();



            if (invokeFinished)
                OnPatternFinished?.Invoke(_current);

            _current = null;
        }
    }
}
