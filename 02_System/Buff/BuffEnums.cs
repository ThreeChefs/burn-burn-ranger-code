public enum BuffStackPolicy
{
    Refresh,    // 시간 리셋
    Stack,      // 중첩
    Replace,    // 교체
    Ignore      // 이미 있으면 무시
}

[System.Flags]
public enum BuffEndCondition
{
    None = 0,
    Time = 1 << 0,
    BreakOnHit = 1 << 1,
}

public enum HpCompareType
{
    LessOrEqual,   // 이하
    GreaterOrEqual // 이상
}