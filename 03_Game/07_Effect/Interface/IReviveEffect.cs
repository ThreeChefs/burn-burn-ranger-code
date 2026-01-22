public interface IReviveEffect : IConditionalEffect
{
    public bool CanTrigger(in BaseEffectContext context);
    public ReviveData GetReviveData();
}
