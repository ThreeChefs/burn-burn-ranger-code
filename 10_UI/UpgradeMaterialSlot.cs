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
        SlotInfo slotInfo = new SlotInfo
        {
            contentCount = count,
        };

        switch (type)
        {
            case WalletType.UpgradeMaterial_Weapon:
                slotInfo.contentSpr = _weaponMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Armor:
                slotInfo.contentSpr = _armorMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Shoes:
                slotInfo.contentSpr = _shoesMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Gloves:
                slotInfo.contentSpr = _glovesMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Belt:
                slotInfo.contentSpr = _beltMaterialSpr;
                break;

            case WalletType.UpgradeMaterial_Necklace:
                slotInfo.contentSpr = _necklaceMaterialSpr;
                break;
        }

        SetSlot(slotInfo);
    }
}
