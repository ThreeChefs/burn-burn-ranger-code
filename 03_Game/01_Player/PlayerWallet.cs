using System;
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
        _wallets = new();

        // todo: 플레이어 초기화 시 저장된 값 불러오기
        foreach (WalletType wallet in Enum.GetValues(typeof(WalletType)))
        {
            _wallets.Add(wallet, new Wallet(0, wallet));
        }

        _wallets[WalletType.Gold].Add(100);
        _wallets[WalletType.Energy].Add(5);
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
