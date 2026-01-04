using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [Header("Monster Data")]
    [SerializeField] private MonsterTypeData monsterdata;
    private StagePlayer target;
    public BaseStat Speed { get; private set; }
    public BaseStat Hp { get; private set; }
    public BaseStat Attack { get; private set; }
    bool isLive;

    Rigidbody2D rb;
    SpriteRenderer spriter;
    [SerializeField] private float hitCooldown = 0.5f;
    private bool _canHit = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        ApplyData(monsterdata);
        target = PlayerManager.Instance.StagePlayer;


    }

    public void ApplyData(MonsterTypeData monsterTypeData)
    {
        if (monsterTypeData == null)
        {
            Debug.LogError($"{name} : MonsterTypeData가 연결되지 않았습니다!");
            enabled = false;
            return;
        }
        monsterdata = monsterTypeData;
        Speed = new BaseStat(monsterTypeData.Get(StatType.Speed), StatType.Speed);
        Hp = new BaseStat(monsterTypeData.Get(StatType.Health), StatType.Health);
        Attack = new BaseStat(monsterTypeData.Get(StatType.Attack), StatType.Attack);
    }
    private void FixedUpdate()
    {

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
            Debug.Log($"{name} hit! data={monsterdata.name}, type={monsterdata.monsterType}, atk={Attack}");
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


        if (!Hp.TryUse(value))
            return;

        Logger.Log($"Enemy HP : {Hp.CurValue}");

        if (Hp.CurValue <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {

        Logger.Log("사망");
        DropItem();
        Destroy(gameObject);
    }


    private void DropItem()
    {
        for (int i = 0; i < monsterdata.dropCount; i++)
        {
            MonsterDropItem.Instance.Spawn(monsterdata.dropItemType, transform.position);
        }
    }
}
