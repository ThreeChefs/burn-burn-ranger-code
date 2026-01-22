
using JetBrains.Annotations;
using UnityEngine;

public class WalletAddButton : BaseButton
{
    [SerializeField] WalletType _walletType;

    protected override void Awake()
    {
        base.Awake();
        _button.onClick.AddListener(AddGem);
    }

    // 테스트용 젬 추가
    public void AddGem()
    {
        PlayerManager.Instance.Wallet[_walletType].Add(1000);
    }
}
