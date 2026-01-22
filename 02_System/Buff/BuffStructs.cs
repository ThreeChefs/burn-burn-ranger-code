using System;

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
