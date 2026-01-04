using UnityEngine;

/// <summary>
/// 기본 플레이어
/// </summary>
public class Player : MonoBehaviour
{
    [Header("SO Data")]
    [SerializeField] private StatData _statData;

    public PlayerCondition Condition { get; protected set; }

    protected virtual void Awake()
    {
        Condition = new(_statData);
    }

    #region 에디터 전용
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        _statData = AssetLoader.FindAndLoadByName<StatData>("PlayerStatData");
    }
#endif
    #endregion
}
