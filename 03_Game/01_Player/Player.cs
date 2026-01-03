using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어
/// </summary>
[RequireComponent(typeof(CapsuleCollider2D))]
public class Player : MonoBehaviour
{
    [Header("움직임")]
    [SerializeField] private float _speed = 5f;
    private Vector2 _inputVector;

    private void Awake()
    {

    }

    public void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Vector2 nextVec = _speed * Time.fixedDeltaTime * _inputVector.normalized;
        Vector2 pos = transform.position;
        Vector2 newPos = pos + nextVec;
        transform.position = newPos;
    }

    private void OnMove(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }
    #region Move

    #endregion

    #region 에디터 전용

    #endregion
}
