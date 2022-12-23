namespace TrafficToll.Internals.ValueObjects;

internal sealed record TollCalculationParameters
{
    public TollCalculationParameters(
        int maximumDailyFee, 
        TimeSpan validTollTime, 
        IEnumerable<TollFeeSpan> tollFeeSpans)
    {
        MaximumDailyFee = maximumDailyFee;
        ValidTollTime = validTollTime;
        TollFeeSpans = tollFeeSpans;
    }

    public int MaximumDailyFee { get; }
    public TimeSpan ValidTollTime { get; }
    public IEnumerable<TollFeeSpan> TollFeeSpans { get; }
}
