using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 스테이지 내부 플레이어
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class StagePlayer : Player
{
    [Header("움직임")]
    [SerializeField] private float _speed = 5f;
    private Vector2 _inputVector;

    private void FixedUpdate()
    {
        Move();
    }

    #region Move
    private void Move()
    {
        Vector2 nextVec = _speed * Time.fixedDeltaTime * _inputVector.normalized;
        IsLeft = nextVec.x > 0;
        Vector2 pos = transform.position;
        Vector2 newPos = pos + nextVec;
        transform.position = newPos;
    }

    private void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }
    #endregion

    #region 에디터 전용
#if UNITY_EDITOR
    protected override void Reset()
    {
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.offset = new Vector2(-0.03598577f, 0.2159152f);
        collider.size = new Vector2(0.805023f, 1.287887f);

        PlayerInput input = GetComponent<PlayerInput>();
        input.actions = AssetLoader.FindAndLoadByName<InputActionAsset>("Player");
        input.notificationBehavior = PlayerNotifications.SendMessages;
    }
#endif
    #endregion
}
