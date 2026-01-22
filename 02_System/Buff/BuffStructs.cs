using System;
using UnityEngine;

public readonly struct BuffInstanceKey : IEquatable<BuffInstanceKey>
{
    private static int Next;
    public readonly int Value;

    private BuffInstanceKey(int value)
    {
        Value = value;
    }

    public static BuffInstanceKey New() => new(Next++);

    public static void ResetGenerator() => Next = 0;

    public bool Equals(BuffInstanceKey other)
    {
        return Value == other.Value;
    }
}

[Serializable]
public struct HpRatioCondition
{
    [field: SerializeField] public HpCompareType CompareType { get; private set; }
    [field: SerializeField][field: Range(0, 1)] public float Ratio { get; private set; }

    public bool Evaluate(float hpRatio)
    {
        return CompareType switch
        {
            HpCompareType.LessOrEqual => hpRatio <= Ratio,
            HpCompareType.GreaterOrEqual => hpRatio >= Ratio,
            _ => false
        };
    }
}
