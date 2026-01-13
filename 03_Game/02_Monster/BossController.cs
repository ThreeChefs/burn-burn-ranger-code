using UnityEngine;

public class BossController : MonoBehaviour   // 보스 모든 패턴의 총괄 패턴명령 
{
    [Header("보스데이터 (SO)")]
    [SerializeField] private MonsterTypeData monsterData;

    [Header("Runtime HP")]
    [SerializeField] private float maxHp;   // SO에서 읽어온 최대 체력
    [SerializeField] private float curHp;   // 현재 체력(데미지 받으면 감소)
    [SerializeField] private float attack;
    public float Attack => attack;
    [Header("Refs")]
    [SerializeField] private BossPatternController patternController;
    private Monster monster;
    [Header("Target")]
    [SerializeField] public Transform target;
    public bool patternLocked { get; private set; }
    public void SetPatternLock(bool locked)
        => patternLocked = locked;


    public Transform Target => target;

    public float MaxHp { get { return maxHp; } }
    public float CurHp { get { return curHp; } }
    public float HpRatio
    {
        get
        {
            return (maxHp <= 0f) ? 0f : (curHp / maxHp);
        }
    }
    public bool IsDead
    {
        get
        {
            return curHp <= 0f;
        }
    }

    private void Awake()
    {
        if (patternController == null)
            patternController = GetComponentInChildren<BossPatternController>(true);
        monster = GetComponent<Monster>();
        ApplyDataSO();
        if (target == null)
        {
            var player = PlayerManager.Instance?.StagePlayer;
            if (player != null)
                target = player.transform;
        }
        if (patternController != null)
            patternController.Bind(this);
    }

    private void Start()
    {

    }

    private void Update()
    {
        // 죽었으면 아무 것도 하지 않음
        if (IsDead) return;
        if (patternLocked) return;
        // 패턴 컨트롤러 없으면 종료
        if (patternController == null) return;

        // 패턴 선택/실행은 PatternController에게 맡김
        patternController.Tick();
    }


    private void LateUpdate()
    {
        // HP 10 감소
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ApplyDamage(10f);
        }

        // HP 50 감소
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ApplyDamage(50f);
        }
    }
    public void ApplyDataSO()
    {

        if (monsterData == null)
        {
            return;
        }
        maxHp = monsterData.Get(StatType.Health);

        // 현재 체력은 최대 체력으로 초기화(풀피 시작)
        curHp = maxHp;
        attack = monsterData.Get(StatType.Attack);

    }
    public void ApplyDamage(float damage)
    {
        if (IsDead) return;

        Debug.Log($"[BossHP] DAMAGE={damage} | BEFORE cur={curHp} / max={maxHp} ({HpRatio:F2})");

        curHp = Mathf.Max(0f, curHp - damage);

        Debug.Log($"[BossHP] AFTER  cur={curHp} / max={maxHp} ({HpRatio:F2})");

        if (IsDead)
            monster.Die();
    }
    private void TestDamage10()
    {
        ApplyDamage(10f);
    }

    [ContextMenu("TEST: Damage 50")]
    private void TestDamage50()
    {
        ApplyDamage(50f);
    }

}
