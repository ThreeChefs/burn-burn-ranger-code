using System.Collections.Generic;

/// <summary>
/// 플레이어 보유 통화(재산) 정보를 관리하는 컨테이너 
/// </summary>
[System.Serializable]
public class PlayerWallet
{
    #region 필드
    private readonly Dictionary<WalletType, Wallet> _wallets;
    public Wallet this[WalletType type] => _wallets[type];      // 인덱서 문법 사용
    #endregion

    #region 초기화 & 파괴
    public PlayerWallet()
    {
        // todo: 플레이어 초기화 시 저장된 값 불러오기
        _wallets = new()
        {
            { WalletType.DungeonKey, new(5, WalletType.DungeonKey) },
            { WalletType.Gold, new(10, WalletType.Gold) },
            { WalletType.Gem, new(0, WalletType.Gem) }
        };
    }

    /// <summary>
    /// [public] Player가 파괴될 때 사용
    /// </summary>
    public void OnDestroy()
    {
        foreach (Wallet wallet in _wallets.Values)
        {
            wallet.OnDestroy();
        }
    }
    #endregion
}
