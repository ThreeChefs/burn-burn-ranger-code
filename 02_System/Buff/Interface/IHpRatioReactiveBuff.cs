/// <summary>
/// Hp를 기준으로 버프를 주기 위한 인터페이스
/// </summary>
public interface IHpRatioReactiveBuff
{
    public bool ShouldBeActive(float hpRatio);
}
