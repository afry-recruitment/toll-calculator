using TrafficToll.Internals.ValueObjects;

namespace TrafficToll.Internals.Services;
internal sealed class TollCalculatorCore
{
    private readonly int _maximumDailyFee;
    private readonly TimeSpan _validTollTime;
    private readonly IEnumerable<TollFeeSpan> _tollFeeSpans;

    public TollCalculatorCore(TollCalculationParameters tollCalculationParameters)
    {
        _maximumDailyFee = tollCalculationParameters.MaximumDailyFee;
        _validTollTime = tollCalculationParameters.ValidTollTime;
        _tollFeeSpans = tollCalculationParameters.TollFeeSpans;
    }

    public int CalculateTollFee(IEnumerable<DateTime> passings)
    {
        var groupedPassings = GroupByValidTollTime(passings, _validTollTime);
        int totalSum = CalculateSum(groupedPassings);
        return totalSum < _maximumDailyFee ? totalSum : _maximumDailyFee;
    }

    private int CalculateSum(IEnumerable<IEnumerable<DateTime>> groupedPassings)
    {
        return groupedPassings.Select(GetHighestPriceInGroup).Sum();
    }

    private int GetHighestPriceInGroup(IEnumerable<DateTime> passings)
    {
        return passings.Select(x => ConvertDateTimeToPrice(x, _tollFeeSpans)).Max();
    }

    internal static int ConvertDateTimeToPrice(DateTime passing, IEnumerable<TollFeeSpan> tollFeeSpans)
    {
        var span = new TimeSpan(passing.Hour, passing.Minute, passing.Second, passing.Millisecond);
        return tollFeeSpans.Single(x => x.Start <= span && span < x.End).TollPrice;
    }

    internal static IEnumerable<IEnumerable<DateTime>> GroupByValidTollTime(IEnumerable<DateTime> timeSpans, TimeSpan validTollTime)
    {
        var groupList = new List<IEnumerable<DateTime>>();

        while (true)
        {
            var startTime = timeSpans.First();
            var group = timeSpans.Where(x => x >= startTime && x < startTime + validTollTime);
            timeSpans = timeSpans.Where(x => !group.Contains(x));
            groupList.Add(timeSpans);

            if (!timeSpans.Any())
                break;
        }

        return groupList;
    }
}