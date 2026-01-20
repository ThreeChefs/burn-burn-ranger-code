/// <summary>
/// 스킬 - 쿠나이
/// </summary>
public class KunaiActiveSkill : ActiveSkill
{
    private void Start()
    {
        ItemInstance weapon = PlayerManager.Instance.Equipment.Equipments[EquipmentType.Weapon];
        if (weapon.ItemClass >= ItemClass.Rare)
        {
            DamageMultiplier = 1.3f;
        }
    }
}
