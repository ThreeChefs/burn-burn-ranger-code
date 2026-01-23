using System;
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
    [field: SerializeField] public Inventory Inventory { get; private set; }
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

    #region 데이터 저장
    /// <summary>
    /// [public] 데이터 로드하기
    /// </summary>
    /// <param name="data"></param>
    public void LoadInventoryData(InventorySaveData inventoryData)
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

    public InventorySaveData SaveInventoryData()
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

        return inventoryData;
    }

    public void LoadWalletData(WalletSaveData walletData)
    {
        for (int i = 0; i < walletData.WalletTypes.Count; i++)
        {
            WalletType type = walletData.WalletTypes[i];
            int value = walletData.Values[i];

            Wallet[type].Add(value);
        }
    }

    public WalletSaveData SaveWalletData()
    {
        WalletSaveData walletData = new();

        foreach (WalletType type in Enum.GetValues(typeof(WalletType)))
        {
            walletData.WalletTypes.Add(type);
            walletData.Values.Add(Wallet[type].Value);
        }

        return walletData;
    }
    #endregion

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
