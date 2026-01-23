using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpButton : BaseButton
{
    [Header("박스 정보")]
    [SerializeField] private int _boxId;
    [SerializeField] private List<BoxWallet> _boxWallets = new();
    private int _boxWalletIndex = 0;

    [Header("버튼 정보")]
    [SerializeField] private Image _moneyIcon;
    [SerializeField] private TextMeshProUGUI _requiredValue;

    private Image _image;

    private PickUpSystem _pickUpSystem;
    private readonly List<ItemInstance> _items = new();

    protected override void Awake()
    {
        base.Awake();
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _pickUpSystem = GameManager.Instance.PickUpSystem;
        foreach (BoxWallet wallet in _boxWallets)
        {
            PlayerManager.Instance.Wallet[wallet.WalletType].OnValueChanged += CheckWallet;
        }
        SetPickButton(0);
    }

    private void OnEnable()
    {
        if (PlayerManager.Instance == null) return;
        foreach (BoxWallet wallet in _boxWallets)
        {
            PlayerManager.Instance.Wallet[wallet.WalletType].OnValueChanged += CheckWallet;
        }
        SetPickButton(0);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _button.onClick.RemoveAllListeners();
        foreach (BoxWallet wallet in _boxWallets)
        {
            PlayerManager.Instance.Wallet[wallet.WalletType].OnValueChanged -= CheckWallet;
        }
    }

    protected override void OnClick()
    {
        if (_pickUpSystem == null)
        {
            return;
        }

        BoxWallet wallet = _boxWallets[_boxWalletIndex];
        if (PlayerManager.Instance.Wallet[wallet.WalletType].TryUse(wallet.RequiredValue))
        {
            for (int i = 0; i < wallet.PickUpCount; i++)
            {
                ItemInstance item = _pickUpSystem.PickUp(_boxId);
                PlayerManager.Instance.Inventory.Add(item);
                _items.Add(item);
            }

            PickUpUI ui = UIManager.Instance.ShowUI(UIName.UI_PickUp) as PickUpUI;
            ui.PickUpItems(_items);
            _items.Clear();

            return;
        }
    }

    private void CheckWallet(int value)
    {
        for (int i = 0; i < _boxWallets.Count; i++)
        {
            BoxWallet wallet = _boxWallets[i];

            if (value > wallet.RequiredValue)
            {
                _boxWalletIndex = i;
                SetPickButton(_boxWalletIndex);
                return;
            }
        }
    }

    private void SetPickButton(int index)
    {
        int value = _boxWallets[index].RequiredValue;
        if (value > PlayerManager.Instance.Wallet[_boxWallets[index].WalletType].Value)
        {
            _image.color = Color.gray;
            _button.enabled = false;
        }
        else
        {
            _image.color = Color.white;
            _button.enabled = true;
        }

        _moneyIcon.sprite = _boxWallets[index].MoneyIcon;
        _requiredValue.text = _boxWallets[index].RequiredValue.ToString();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _moneyIcon = transform.FindChild<Image>("Image - Money");
        _requiredValue = transform.FindChild<TextMeshProUGUI>("Text (TMP) - RequiredValue");
    }
#endif
}

[System.Serializable]
public struct BoxWallet
{
    [field: SerializeField] public WalletType WalletType { get; private set; }
    [field: SerializeField] public Sprite MoneyIcon { get; private set; }
    [field: SerializeField] public int RequiredValue { get; private set; }
    [field: SerializeField] public int PickUpCount { get; private set; }
}