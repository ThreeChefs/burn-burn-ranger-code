using UnityEngine;

/// <summary>
/// 기본 플레이어
/// </summary>
public class Player : MonoBehaviour
{
    [Header("이미지")]
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

    public PlayerCondition Condition { get; protected set; }

    protected virtual void Awake()
    {

    }

    #region sprite 관리
    private void Flip()
    {

    }
    #endregion

    #region 에디터 전용
#if UNITY_EDITOR
    protected virtual void Reset()
    {
    }
#endif
    #endregion
}
