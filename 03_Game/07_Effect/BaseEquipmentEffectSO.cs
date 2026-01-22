using UnityEngine;

public abstract class BaseEquipmentEffectSO : ScriptableObject
{
    public abstract EquipmentEffectInstance CreateInstance();
}

public abstract class EquipmentEffectInstance
{
    public BuffInstanceKey Key { get; }
    protected readonly BaseEquipmentEffectSO source;

    protected EquipmentEffectInstance(BaseEquipmentEffectSO source)
    {
        Key = BuffInstanceKey.New();
        this.source = source;
    }

    public virtual void OnStageStart(BuffSystem system) { }
}