using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : BaseUI
{
    [Title("플레이어 정보")]
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] Slider _levelExp;

    [Title("재화 표시 오브젝트")]
    [SerializeField] GameObject _gold;
    [SerializeField] GameObject _gem;
    [SerializeField] GameObject _energy;


    [Title("재화")]
    [SerializeField] BigIntText _goldText;
    [SerializeField] BigIntText _gemText;
    [SerializeField] BigIntText _energyText;



    private void Start()
    {
        ChangeBottomMenu(BottomBarMenuType.Home);   // 처음엔 Home화면

        _nameText.text = "플레이어예요"; //


        // 이벤트 연결

        // 레벨
        SetLevelText(PlayerManager.Instance.Condition.GlobalLevel.Level);
        PlayerManager.Instance.Condition.GlobalLevel.OnLevelChanged += SetLevelText;

        // 경험치
        SetExpSliderValue(PlayerManager.Instance.Condition.GlobalLevel.CurrentExp / PlayerManager.Instance.Condition.GlobalLevel.RequiredExp);
        PlayerManager.Instance.Condition.GlobalLevel.OnExpChanged += SetExpSliderValue;

        // 골드
        _goldText.SetValue(PlayerManager.Instance.Wallet[WalletType.Gold].Value);
        PlayerManager.Instance.Wallet[WalletType.Gold].OnValueChanged += _goldText.SetValue;

        // 젬
        _gemText.SetValue(PlayerManager.Instance.Wallet[WalletType.Gem].Value);
        PlayerManager.Instance.Wallet[WalletType.Gem].OnValueChanged += _gemText.SetValue;

        // 에너지

    }


    void SetLevelText(int level)
    {
        _levelText.text = level.ToString();
    }

    void SetExpSliderValue(float value)
    {
        _levelExp.value = value;
    }


    public void ChangeBottomMenu(BottomBarMenuType type)
    {
        _gem.SetActive(false);
        _gold.SetActive(false);
        _energy.SetActive(false);

        switch (type)
        {
            case BottomBarMenuType.Shop:
                _gem.SetActive(true);
                _gold.SetActive(true);
                break;

            case BottomBarMenuType.Equipment:
                _gem.SetActive(true);
                _gold.SetActive(true);
                break;

            case BottomBarMenuType.Home:
                _energy.SetActive(true);
                _gold.SetActive(true);
                break;

            case BottomBarMenuType.Growth:
                _gem.SetActive(true);
                _gold.SetActive(true);
                break;
        }

    }

    private void OnDestroy()
    {
        PlayerManager.Instance.Wallet[WalletType.Gold].OnValueChanged -= _goldText.SetValue;
    }

}
