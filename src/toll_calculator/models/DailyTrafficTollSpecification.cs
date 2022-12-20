namespace toll_calculator.models;

internal sealed class DailyTrafficTollSpecification
{
    public DailyTrafficTollSpecification(int maximumDailyFee, TimeSpan validTollTime, Dictionary<int, int> priceMapping, TollTimePrize[] dailyTollTimePrizes)
    {
        MaximumDailyFee = maximumDailyFee;
        ValidTollTime = validTollTime;
        PriceMapping = priceMapping;
        DailyTollTimePrizes = dailyTollTimePrizes;
    }

    public int MaximumDailyFee { get; }
    public TimeSpan ValidTollTime { get; }
    public Dictionary<int, int> PriceMapping { get; }
    public TollTimePrize[] DailyTollTimePrizes { get; }
}