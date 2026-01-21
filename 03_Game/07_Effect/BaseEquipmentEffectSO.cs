using UnityEngine;

public abstract class BaseEquipmentEffectSO : ScriptableObject
{
    public abstract EquipmentEffectInstance CreateInstance();
}

public abstract class EquipmentEffectInstance
{
    protected readonly BaseEquipmentEffectSO source;

    protected EquipmentEffectInstance(BaseEquipmentEffectSO source)
    {
        this.source = source;
    }
}