using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [Header("Monster Data")]
    [SerializeField] private MonsterTypeData monsterdata;


    public Rigidbody2D target;

    private float _speed;
    private float _hp;
    private float _attack;
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
        ApplyData();
    }

    private void ApplyData()
    {
        if (monsterdata == null)
        {
            Debug.LogError($"{name} : MonsterTypeData가 연결되지 않았습니다!");
            enabled = false;
            return;
        }
        _speed = monsterdata.baseSpeed;
        _hp = monsterdata.baseHealth;
        _attack = monsterdata.baseAttack;
    }
    private void FixedUpdate()
    {

        if (target == null)
            return;

        Vector2 directionVector = target.position - rb.position;
        Vector2 nextVector = directionVector.normalized * _speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVector); //플레이어 키입력값을 더한이동 + 몬스터방향값을 더한이동
        rb.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        spriter.flipX = target.position.x < rb.position.x;
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_canHit)
        {
            return;
        }

        if (collision.collider.TryGetComponent<StagePlayer>(out var player))
        {
            player.TakeDamage(_attack);
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
        if (_hp <= 0f)
        {
            return;
        }
        _hp -= value;
        if (_hp > 0f)
        {
            Logger.Log($"Enemy HP : {_hp}");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        Logger.Log("뒤짐");
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
