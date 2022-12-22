namespace TrafficToll.Internals.ValueObjects;

internal sealed record TollCalculationParameters
{
    public TollCalculationParameters(
        int maximumDailyFee, 
        TimeSpan validTollTime, 
        IEnumerable<TollFeeSpan> tollFeeSpans,
        IEnumerable<(int year, int month, int day)> tollFreeDates)
    {
        MaximumDailyFee = maximumDailyFee;
        ValidTollTime = validTollTime;
        TollFeeSpans = tollFeeSpans;
        TollFreeDates = tollFreeDates;
    }

    public int MaximumDailyFee { get; }
    public TimeSpan ValidTollTime { get; }
    public IEnumerable<TollFeeSpan> TollFeeSpans { get; }
    public IEnumerable<(int year, int month, int day)> TollFreeDates { get; }
}