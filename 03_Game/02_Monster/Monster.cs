using System;
using System.Collections;
using UnityEngine;

public class Monster : PoolObject, IDamageable, IKnockbackable
{
    [Header("Monster Data")]
    [SerializeField] private MonsterTypeData monsterdata;
    public StagePlayer target;
    public BaseStat Speed { get; private set; }
    public BaseStat Hp { get; private set; }
    public BaseStat Attack { get; private set; }
    bool isLive;
    private float _maxHp;
    protected Rigidbody2D rb;
    SpriteRenderer spriter;
    [SerializeField] private float hitCooldown = 0.5f;
    private bool _canHit = true;
    public event Action<Monster> onDieAction;
    private BossController bossController;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.1f;
    [SerializeField] private bool Knockbackable = true;
    private bool _isKnockback;
    private Coroutine _knockbackCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>(true);
        bossController = GetComponent<BossController>();
    }
    protected override void OnEnableInternal()
    {
        ResetForPoolSpawn(this);
    }
    private void Start()
    {
        ApplyData(monsterdata);
        target = PlayerManager.Instance.StagePlayer;


    }


    public void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Monster");
    }
    public void ApplyData(MonsterTypeData monsterTypeData)
    {
        if (monsterTypeData == null)
        {
            Debug.LogError($"[{name}] MonsterTypeData is NULL");
            return;
        }
        monsterdata = monsterTypeData;
        Speed = new BaseStat(monsterTypeData.Get(StatType.Speed), StatType.Speed);
        Hp = new BaseStat(monsterTypeData.Get(StatType.Health), StatType.Health);
        Attack = new BaseStat(monsterTypeData.Get(StatType.Attack), StatType.Attack);
        _maxHp = Hp.MaxValue;
    }
    protected virtual void FixedUpdate()
    {

        if (bossController != null && bossController.patternLocked)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (target == null)
            return;

        Vector2 directionVector = (Vector2)target.transform.position - rb.position;
        Vector2 nextVector = directionVector.normalized * Speed.CurValue * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVector); //플레이어 키입력값을 더한이동 + 몬스터방향값을 더한이동
        rb.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {

        if (target == null) return;
        spriter.flipX = target.transform.position.x < rb.position.x;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_canHit)
        {
            return;
        }

        if (collision.collider.TryGetComponent<StagePlayer>(out var player))
        {

            player.TakeDamage(Attack.CurValue);
            StartCoroutine(HitCooldown());
        }
    }

    private IEnumerator HitCooldown()
    {
        _canHit = false;
        yield return new WaitForSeconds(hitCooldown);
        _canHit = true;
    }

    public void TakeDamage(float value)
    {
        if (Hp == null)
            return;

        if (Hp.TryUse(value))
        {
            if (Hp.CurValue == 0)
            {
                Die();
            }
        }
        else
        {
            Die();
        }
        DamageText damageText = CommonPoolManager.Instance.Spawn<DamageText>(CommonPoolIndex.DamageText, transform.position);
        damageText?.Init(value);
    }


    public virtual void Die()
    {

        DropItem();
        onDieAction?.Invoke(this);
        // Destroy(gameObject);
    }


    protected virtual void DropItem()
    {
        for (int i = 0; i < monsterdata.dropCount; i++)
        {
            MonsterDropItem.Instance.Spawn(monsterdata.dropItemType, transform.position);
        }
    }

    public void BombDie()
    {
        Die();
    }

    public void ResetForPoolSpawn(PoolObject pool)
    {

        if (Hp != null)
            Hp.ResetCurValue();


        _canHit = true;
        isLive = true;


        rb.velocity = Vector2.zero;


        gameObject.layer = LayerMask.NameToLayer("Monster");


    }

    public void ApplyKnockback(Vector2 position, float force)
    {
        if (rb == null)
        {
            return;
        }
        Vector2 direction = ((Vector2)transform.position - position).normalized;
        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.up;

        if (_knockbackCoroutine != null)
        {
            StopCoroutine(_knockbackCoroutine);
        }
        _knockbackCoroutine = StartCoroutine(KnockbackRoutine(direction, force));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force)
    {
        _isKnockback = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        _isKnockback = false;
        _knockbackCoroutine = null;
    }
}
