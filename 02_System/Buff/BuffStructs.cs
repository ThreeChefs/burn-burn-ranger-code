using UnityEngine;

[System.Serializable]
public struct BuffKey : System.IEquatable<BuffKey>
{
    [UnityEngine.SerializeField] private int _value;
    public readonly int Value => _value;

    public BuffKey(int value) { _value = value; }
    public bool Equals(BuffKey other) { return Value == other.Value; }
}

[System.Serializable]
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
