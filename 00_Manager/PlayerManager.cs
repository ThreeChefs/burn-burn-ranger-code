using UnityEngine;

public class PlayerManager : GlobalSingletonManager<PlayerManager>
{
    [SerializeField] private StagePlayer _stagePlayerPrefab;
    public StagePlayer StagePlayer { get; private set; }

    [Header("SO Data")]
    [SerializeField] private StatData _statData;

    // POCO Class
    public PlayerCondition Condition { get; private set; }
    public PlayerWallet Wallet { get; private set; }
    public Inventory Inventory { get; private set; }
    public Equipment Equipment { get; private set; }

    protected override void Init()
    {
        base.Init();

        Condition = new(_statData);
        Wallet = new();
        Inventory = new();
    }

    private void OnDestroy()
    {
        Condition.OnDestroy();
        Wallet.OnDestroy();
        Inventory.OnDestroy();
    }

    /// <summary>
    /// [public] 스테이지에서 플레이어를 소환하기 위한 용도
    /// </summary>
    public StagePlayer SpawnPlayer()
    {
        StagePlayer = Instantiate(_stagePlayerPrefab);
        return StagePlayer;
    }

    #region 에디터 전용
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        _stagePlayerPrefab = AssetLoader.FindAndLoadByName("StagePlayer").GetComponent<StagePlayer>();
        _statData = AssetLoader.FindAndLoadByName<StatData>("PlayerStatData");
    }
#endif
    #endregion
}
