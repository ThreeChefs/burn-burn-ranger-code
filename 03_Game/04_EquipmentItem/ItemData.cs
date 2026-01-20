using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 아이템 원본 데이터 (코드에서 수정 x)
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item/Item")]
public class ItemData : ScriptableObject
{
    [field: Header("Info")]
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: Header("Equipment")]
    [field: ShowIf("Type", ItemType.Equipment)]
    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
    [field: ShowIf("Type", ItemType.Equipment)]
    [field: SerializeField] public EquipmentEffectData[] Equipments { get; private set; }

    public override string ToString()
    {
        return "{" +
            $"Id: {Id} " +
            $"Name: {DisplayName} " +
            $"Description: {Description} " +
            $"EquipmentType: {EquipmentType} " +
            "}";
    }

#if UNITY_EDITOR
    private void Reset()
    {
        Id = ItemUtils.GetItemNumber();
    }

    [Button("아이템 id 지정")]
    private void SetNumber(int num)
    {
        ItemUtils.SetItemNumber(num);
        SetNumber();
    }

    public void SetNumber()
    {
        Id = ItemUtils.GetItemNumber();
    }

    [Button("아이템 id 전체 지정")]
    private void SetNumberAllItem()
    {
        ItemUtils.SetItemNumber(0);
        AssetLoader.FindAndLoadAllByType<ItemData>().ForEach(item => item.SetNumber());
    }
#endif
}

[System.Serializable]
public class EquipmentEffectData
{
    [field: Header("공통")]
    [field: SerializeField] public ItemClass UnlockClass { get; private set; }
    [field: SerializeField] public EquipmentEffectType EffectType { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: Header("스텟")]
    [field: ShowIf("EffectType", EquipmentEffectType.Stat)]
    [field: SerializeField] public EffectApplyType ApplyType { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Stat)]
    [field: SerializeField] public StatType Stat { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Stat)]
    [field: SerializeField] public int Value { get; private set; }

    [field: Header("스킬")]
    [field: ShowIf("EffectType", EquipmentEffectType.Skill)]
    [field: SerializeField] public SkillData SkillData { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Skill)]
    [field: SerializeField] public int SkillLevel { get; private set; }

    [field: Header("조건부 버프")]
    [field: ShowIf("EffectType", EquipmentEffectType.Buff)]
    [field: SerializeField] public EffectTriggerType TriggerType { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Buff)]
    [field: SerializeField] public EffectTargetType TargetType { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Buff)]
    [field: SerializeField] public BuffType BuffType { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Buff)]
    [field: SerializeField] public float Duration { get; private set; }
    [field: ShowIf("EffectType", EquipmentEffectType.Buff)]
    [field: SerializeField] public int RequiredTriggerCount { get; private set; }

    public EquipmentEffectData()
    {
        ApplyType = EffectApplyType.Percent;
    }
}