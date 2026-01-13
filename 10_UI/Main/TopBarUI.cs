using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : BaseUI
{
    [Title("플레이어 정보")]
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] Slider _levelExp;

    [Title("재화")]
    [SerializeField] BigIntText _goldText;

    private void Start()
    {
        _nameText.text = "플레이어예요"; //

        SetLevelText(PlayerManager.Instance.Condition.GlobalLevel.Level);
        PlayerManager.Instance.Condition.GlobalLevel.OnLevelChanged += SetLevelText;

        SetExpSliderValue(PlayerManager.Instance.Condition.GlobalLevel.CurrentExp / PlayerManager.Instance.Condition.GlobalLevel.RequiredExp);
        PlayerManager.Instance.Condition.GlobalLevel.OnExpChanged += SetExpSliderValue;

        _goldText.SetValue(PlayerManager.Instance.Wallet[WalletType.Gold].Value);
        PlayerManager.Instance.Wallet[WalletType.Gold].OnValueChanged += _goldText.SetValue;
    }


    void SetLevelText(int level)
    {
        _levelText.text = level.ToString();
    }

    void SetExpSliderValue(float value)
    {
        _levelExp.value = value;
    }


    private void OnDestroy()
    {
        PlayerManager.Instance.Wallet[WalletType.Gold].OnValueChanged -= _goldText.SetValue;
    }

}
