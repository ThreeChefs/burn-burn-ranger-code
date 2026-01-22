/// <summary>
/// n 마리 죽일 때마다
/// </summary>
public interface IThresholdEffect : IEquipmentEffect
{
    public bool TryConsume(in KillEffectContext context);
}
