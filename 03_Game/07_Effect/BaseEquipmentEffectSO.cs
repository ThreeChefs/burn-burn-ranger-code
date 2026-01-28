using UnityEngine;

public abstract class BaseEquipmentEffectSO : ScriptableObject
{
    [field: SerializeField] public BuffEndCondition BuffEndType { get; private set; }
    public abstract EquipmentEffectInstance CreateInstance();
}

public abstract class EquipmentEffectInstance
{
    public BuffInstanceKey Key { get; }
    protected BuffEndCondition endCondition;
    protected readonly BaseEquipmentEffectSO source;

    protected EquipmentEffectInstance(BaseEquipmentEffectSO source)
    {
        Key = BuffInstanceKey.New();
        endCondition = source.BuffEndType;
        this.source = source;
    }

    public virtual void OnStageStart(BuffSystem system) { }
}