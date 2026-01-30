using Sirenix.OdinInspector;
using System.Collections.Generic;
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
    [field: SerializeReference] public List<EquipmentEffectData> Equipments { get; private set; }

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
public abstract class EquipmentEffectData
{
    [field: BoxGroup("공통")]
    [field: SerializeField] public ItemClass UnlockClass { get; private set; }
    [field: BoxGroup("공통")]
    [field: MultiLineProperty]
    [field: SerializeField] public string Description { get; private set; }
}

[System.Serializable]
public class StatEffectData : EquipmentEffectData
{
    [field: BoxGroup("스텟")]
    [field: EnumToggleButtons]
    [field: SerializeField] public EffectApplyType ApplyType { get; private set; }
    [field: HorizontalGroup("스텟/", width: 0.8f)]
    [field: HideLabel]
    [field: SerializeField] public StatType Stat { get; private set; }
    [field: HorizontalGroup("스텟/", width: 0.2f)]
    [field: HideLabel]
    [field: SerializeField] public int Value { get; private set; }
}

[System.Serializable]
public class SkillEffectData : EquipmentEffectData
{
    [field: BoxGroup("스킬 데이터 | 레벨")]
    [field: HorizontalGroup("스킬 데이터 | 레벨/", width: 0.8f)]
    [field: SerializeField] public SkillData SkillData { get; private set; }
    [field: HorizontalGroup("스킬 데이터 | 레벨/", width: 0.2f)]
    [field: HideLabel]
    [field: SerializeField] public int SkillLevel { get; private set; }
}

[System.Serializable]
public class BuffEffectData : EquipmentEffectData
{
    [field: BoxGroup("조건부 버프")]
    [field: SerializeField] public EffectTriggerType TriggerType { get; private set; }
    [field: BoxGroup("조건부 버프")]
    [field: SerializeField] public EffectTargetType TargetType { get; private set; }
    [field: BoxGroup("조건부 버프")]
    [field: Tooltip("스킬 효과 SO")]
    [field: SerializeField] public BaseEquipmentEffectSO EffectSO { get; private set; }
}