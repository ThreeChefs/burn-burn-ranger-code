using System;

/// <summary>
/// 커스텀 에러 모음
/// </summary>

#region 스텟
/// <summary>
/// 스탯 관련 기본 예외
/// </summary>
public class StatDataException : Exception
{
    public StatType StatType { get; }

    public StatDataException(StatType type, string message)
        : base($"[StatType: {type}] {message}")
    {
        StatType = type;
    }
}

/// <summary>
/// 딕셔너리 변환 시 필수 키가 누락되었을 경우
/// </summary>
public class StatMissingKeyException : StatDataException
{
    public StatMissingKeyException(StatType type)
        : base(type, "필수 스탯이 누락되어 있습니다.") { }
}

/// <summary>
/// 스탯 값이 음수일 경우
/// </summary>
public class StatNegativeValueException : StatDataException
{
    public StatNegativeValueException(StatType type, float value)
        : base(type, $"값이 음수입니다. - {value}") { }
}
#endregion