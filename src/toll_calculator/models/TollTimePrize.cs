namespace toll_calculator.models;

internal sealed class TollTimePrize
{
    public TollTimePrize(TimeSpan start, TimeSpan end, int feeType)
    {
        Start = start;
        End = end;
        FeeType = feeType;
    }

    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public int FeeType { get; }
}
