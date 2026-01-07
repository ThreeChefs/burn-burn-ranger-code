using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 스테이지 내부 플레이어
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class StagePlayer : MonoBehaviour, IDamageable
{
    // 캐싱
    public PlayerCondition Condition { get; private set; }
    private PlayerStat _health;
    private PlayerStat _heal;

    // 레벨
    public LevelSystem StageLevel { get; private set; }

    // 움직임
    private PlayerStat _speed;
    private Vector2 _inputVector;

    // 이미지
    private bool _isLeft;
    protected bool IsLeft
    {
        get { return _isLeft; }
        set
        {
            if (_isLeft != value)
            {
                _isLeft = value;
                Flip();
            }
        }
    }

    // 이벤트
    public event Action OnDieAction;

    #region Unity API
    private void Awake()
    {
        StageLevel = new(1, 0f);
    }

    private void Start()
    {
        Condition = PlayerManager.Instance.Condition;
        _speed = Condition[StatType.Speed];
        _health = Condition[StatType.Health];
        _heal = Condition[StatType.Heal];
    }

    private void Update()
    {
        if (_heal.MaxValue != 0)
        {
            _health.Add(_health.MaxValue * _heal.MaxValue);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDestroy()
    {
        StageLevel.OnDestroy();
    }
    #endregion

    #region Move
    private void Move()
    {
        Vector2 nextVec = _speed.MaxValue * Time.fixedDeltaTime * _inputVector.normalized;
        IsLeft = nextVec.x > 0;
        Vector2 pos = transform.position;
        Vector2 newPos = pos + nextVec;
        transform.position = newPos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _inputVector = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _inputVector = Vector2.zero;
        }
    }
    #endregion

    #region sprite 관리
    private void Flip()
    {

    }
    #endregion

    public void TakeDamage(float value)
    {
        PlayerStat health = Condition[StatType.Health];
        if (health.TryUse(value * (1 - Condition[StatType.DamageReduction].MaxValue)))
        {
            Logger.Log($"플레이어 hp: {health.CurValue} / {health.MaxValue}");

            if (health.CurValue == 0)
            {
                Logger.Log("플레이어 DIE");
                OnDieAction?.Invoke();
            }
        }
        else
        {
            Logger.Log("플레이어 DIE");
            OnDieAction?.Invoke();
        }
    }

    #region 레벨
    public void AddExp(float exp)
    {
        StageLevel.AddExp(exp * Condition[StatType.AddEXP].MaxValue);
    }
    #endregion

    #region 에디터 전용
#if UNITY_EDITOR
    private void Reset()
    {
        TryGetComponent<Rigidbody2D>(out var rigidbody2D);
        rigidbody2D.gravityScale = 0;

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.offset = new Vector2(-0.03598577f, 0.2159152f);
        collider.size = new Vector2(0.805023f, 1.287887f);

        PlayerInput input = GetComponent<PlayerInput>();
        input.actions = AssetLoader.FindAndLoadByName<InputActionAsset>("Player");
        input.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
    }
#endif
    #endregion
}
