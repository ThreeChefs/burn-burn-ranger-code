using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 스테이지 내부 플레이어
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class StagePlayer : Player, IDamageable
{
    [Header("움직임")]
    private PlayerStat _speed;
    private Vector2 _inputVector;

    protected override void Awake()
    {
        base.Awake();

        _speed = Condition[StatType.Speed];
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Move
    private void Move()
    {
        Vector2 nextVec = _speed.CurValue * Time.fixedDeltaTime * _inputVector.normalized;
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

    public void TakeDamage(float value)
    {
        PlayerStat health = Condition[StatType.Health];
        if (health.TryUse(value))
        {
            Logger.Log($"플레이어 hp: {health.CurValue} / {health.MaxValue}");

        }
        else
        {
            Logger.Log("플레이어 DIE");
        }
    }

    #region 에디터 전용
#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

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
