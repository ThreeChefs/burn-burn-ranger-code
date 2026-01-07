using System;

/// <summary>
/// 재화 관리용 클래스
/// </summary>
public class Wallet
{
    // 필드
    public int Value { get; private set; }
    private WalletType _type;

    // 이벤트
    public event Action<int> OnValueChanged;

    public Wallet(int value, WalletType type)
    {
        Value = value;
        _type = type;
    }

    /// <summary>
    /// [public] 초기화 함수
    /// </summary>
    public void OnDestroy()
    {
        OnValueChanged = null;
    }

    /// <summary>
    /// [public] 특정 값 만큼 재화 사용 시도. 변경 다음 OnValueChanged 호출
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool TryUse(int amount)
    {
        if (Value < amount)
        {
            Logger.Log($"{_type} 부족");
            return false;
        }

        Value -= amount;
        OnValueChanged?.Invoke(Value);
        return true;
    }

    /// <summary>
    /// [public] 특정 값 만큼 재화 획득
    /// </summary>
    /// <param name="amount"></param>
    public void Add(int amount)
    {
        Value += amount;
        OnValueChanged?.Invoke(Value);
    }
}
