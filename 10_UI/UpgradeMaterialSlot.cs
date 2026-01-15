using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMaterialSlot : BaseSlot
{
    [SerializeField] Sprite _weaponMaterialSpr;
    [SerializeField] Sprite _armorMaterialSpr;
    [SerializeField] Sprite _shoesMaterialSpr;
    [SerializeField] Sprite _glovesMaterialSpr;
    [SerializeField] Sprite _beldMaterialSpr;
    [SerializeField] Sprite _necklaceMaterialSpr;

    public void SetSlot(WalletType type, int count)
    {
        switch (type)
        {
            case WalletType.UpgradeMaterial_Weapon:
                break;
            case WalletType.UpgradeMaterial_Armor:
                break;
            case WalletType.UpgradeMaterial_Shoes:
                break;
            case WalletType.UpgradeMaterial_Gloves:
                break;
            case WalletType.UpgradeMaterial_Belt:
                break;
            case WalletType.UpgradeMaterial_Necklace:
                break;
        }
    }
}
