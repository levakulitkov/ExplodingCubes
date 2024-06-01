using System;

[Serializable]
public struct MinMaxRange
{
    public int MinInclusive;
    public int MaxExclusive;

    public MinMaxRange(int minInclusive, int maxExclusive)
    {
        MinInclusive = minInclusive;
        MaxExclusive = maxExclusive;
    }
}