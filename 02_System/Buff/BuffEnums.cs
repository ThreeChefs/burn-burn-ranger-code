public enum BuffStackPolicy
{
    Refresh,    // 시간 리셋
    Stack,      // 중첩
    Replace,    // 교체
    Ignore      // 이미 있으면 무시
}

public enum HpCompareType
{
    LessOrEqual,   // 이하
    GreaterOrEqual // 이상
}