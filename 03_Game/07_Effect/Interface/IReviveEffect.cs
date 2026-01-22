public interface IReviveEffect : IEquipmentEffect
{
    public bool CanTrigger(in BaseEffectContext context);
    public ReviveData GetReviveData();
}
