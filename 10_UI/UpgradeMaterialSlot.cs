using Sirenix.OdinInspector;
using UnityEngine;

public class UpgradeMaterialSlot : BaseSlot
{
    [Title("업그레이드 스프라이트 세팅")]
    [SerializeField] Sprite _weaponMaterialSpr;
    [SerializeField] Sprite _armorMaterialSpr;
    [SerializeField] Sprite _shoesMaterialSpr;
    [SerializeField] Sprite _glovesMaterialSpr;
    [SerializeField] Sprite _beltMaterialSpr;
    [SerializeField] Sprite _necklaceMaterialSpr;

    public void SetSlot(WalletType type, int count)
    {
        countText.text = count.ToString();

        switch (type)
        {
            case WalletType.UpgradeMaterial_Weapon:
                iconImg.sprite = _weaponMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Armor:
                iconImg.sprite = _armorMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Shoes:
                iconImg.sprite = _shoesMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Gloves:
                iconImg.sprite = _glovesMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Belt:
                iconImg.sprite= _beltMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Necklace:
                iconImg.sprite = _necklaceMaterialSpr;
                break;
        }

        
        iconImg.gameObject.SetActive(true);
    }
}
