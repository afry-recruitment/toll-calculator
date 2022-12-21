namespace TrafficToll.Internals.DataAccess.Models;

internal sealed class TrafficTollSpecification
{
    public TrafficTollSpecification(
        DateTime validFrom,
        DateTime validUntil,
        int maximumDailyFee,
        Dictionary<int, int> priceMapping,
        TimeSpan validTollTime,
        TollTimePeriod[] dailyTollTimePrizes,
        DateTime[] tollFreeDates,
        int[] tollFreeVehicleTypes)
    {
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        MaximumDailyFee = maximumDailyFee;
        ValidTollTime = validTollTime;
        PriceMapping = priceMapping;
        DailyTollTimePrizes = dailyTollTimePrizes;
        TollFreeDates = tollFreeDates;
        TollFreeVehicleTypes = tollFreeVehicleTypes;
    }

    public DateTime ValidFrom { get; }
    public DateTime ValidUntil { get; }
    public int MaximumDailyFee { get; }
    public TimeSpan ValidTollTime { get; }
    public Dictionary<int, int> PriceMapping { get; }
    public TollTimePeriod[] DailyTollTimePrizes { get; }
    public DateTime[] TollFreeDates { get; }
    public int[] TollFreeVehicleTypes { get; }
}