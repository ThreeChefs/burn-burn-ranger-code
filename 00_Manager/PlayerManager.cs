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
        Equipment = new(Condition);
    }

    private void OnDestroy()
    {
        Condition?.OnDestroy();
        Wallet?.OnDestroy();
        Inventory?.OnDestroy();
        Equipment?.OnDestroy();
    }

    /// <summary>
    /// [public] 스테이지에서 플레이어를 소환하기 위한 용도
    /// </summary>
    public StagePlayer SpawnPlayer()
    {
        StagePlayer = Instantiate(_stagePlayerPrefab);
        return StagePlayer;
    }

    /// <summary>
    /// [public] 데이터 로드하기
    /// </summary>
    /// <param name="data"></param>
    public void LoadData(InventorySaveData inventoryData)
    {
        foreach (ItemSaveData itemData in inventoryData.List)
        {
            var item = new ItemInstance(itemData.ItemClass, itemData.Id, itemData.Count, itemData.Level);
            Inventory.Add(item);
            if (itemData.IsEquipped)
            {
                Equipment.Equip(item);
            }
        }
    }

    public InventorySaveData SaveData()
    {
        InventorySaveData inventoryData = new();

        foreach (ItemInstance item in Inventory.Items)
        {
            inventoryData.List.Add(new ItemSaveData(
                item.ItemClass,
                item.ItemData.Id,
                item.Count,
                item.Level,
                Equipment.IsEquip(item)));
        }

        Logger.Log($"아이템 {inventoryData.List[2].ItemClass} {inventoryData.List[2].IsEquipped} {inventoryData.List[2].Level}");
        return inventoryData;
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
