using UnityEngine;

public class PlayerManager : GlobalSingletonManager<PlayerManager>
{
    [SerializeField] private GameObject _stagePlayerPrefab;
    public StagePlayer StagePlayer { get; private set; }

    [Header("SO Data")]
    [SerializeField] private StatData _statData;

    public PlayerCondition Condition { get; private set; }
    public PlayerWallet Wallet { get; private set; }

    protected override void Init()
    {
        base.Init();

        Condition = new(_statData);
        Wallet = new();
    }

    private void OnDestroy()
    {
        Condition.OnDestroy();
        Wallet.OnDestroy();
    }

    /// <summary>
    /// [public] 스테이지에서 플레이어를 소환하기 위한 용도
    /// </summary>
    public StagePlayer SpawnPlayer()
    {
        StagePlayer = Instantiate(_stagePlayerPrefab).GetComponent<StagePlayer>();
        if (Camera.main.TryGetComponent<FollowCamera>(out var camera))
        {
            camera.ConnectPlayer();
        }
        return StagePlayer;
    }

    #region 에디터 전용
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        _stagePlayerPrefab = AssetLoader.FindAndLoadByName("StagePlayer");
        _statData = AssetLoader.FindAndLoadByName<StatData>("PlayerStatData");
    }
#endif
    #endregion
}
