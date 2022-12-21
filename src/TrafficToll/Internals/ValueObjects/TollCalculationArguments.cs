using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Exceptions;
using TrafficToll.Internals.Models;

namespace TrafficToll.Internals.ValueObjects;

internal sealed record TollCalculationArguments
{
    public int MaximumDailyFee { get; }
    public TimeSpan ValidTollTime { get; }
    public IEnumerable<TollFeeSpan> TollFeeSpans { get; }

    public TollCalculationArguments(int maximumDailyFee, TimeSpan validTollTime, IEnumerable<TollTimePeriod> dailyTollTimePrizes, Dictionary<int, int> priceMapping)
    {
        EnsureTrafficTypesExist(priceMapping);
        var tollFeeSpans = CreateTollFeeSpans(dailyTollTimePrizes, priceMapping);

        MaximumDailyFee = maximumDailyFee;
        ValidTollTime = validTollTime;
        TollFeeSpans = tollFeeSpans;
    }

    private static IEnumerable<TollFeeSpan> CreateTollFeeSpans(IEnumerable<TollTimePeriod> dailyTollTimePrizes, Dictionary<int, int> trafficTypePrizeDictionary)
    {
        return dailyTollTimePrizes.Select(x => new TollFeeSpan(x.Start, x.End, trafficTypePrizeDictionary[x.TollTrafficType]));
    }

    private static void EnsureTrafficTypesExist(Dictionary<int, int> intPrizeDictionary)
    {
        var storedKeys = intPrizeDictionary.Keys.OrderBy(x => x);
        var applicationKeys = Enum.GetValues<TollTrafficType>().OfType<int>();

        if (NotAllStoredKeysIsRepresentedInApplication(storedKeys, applicationKeys))
            throw new InconsistentTrafficTollDataException($"{nameof(TollTrafficType)}");
    }

    private static bool NotAllStoredKeysIsRepresentedInApplication(IEnumerable<int> storedKeys, IEnumerable<int> applicationKeys)
    {
        return applicationKeys.Any(x => !storedKeys.Contains(x));
    }
}