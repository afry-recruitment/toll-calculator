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
        var groupedPassings = TollTimeCalculator.GroupTollPassings(passings, _validTollTime);
        var totalSum = groupedPassings.Select(x => CalculateSum(x)).Sum();
        return totalSum < _maximumDailyFee ? totalSum : _maximumDailyFee;
    }

    private int CalculateSum(IEnumerable<IEnumerable<DateTime>> groupedPassings)
    {
        return groupedPassings.Select(GetHighestPriceInGroup).Sum();
    }

    private int GetHighestPriceInGroup(IEnumerable<DateTime> passings)
    {
        var prices = passings.Select(x => ConvertDateTimeToPrice(x, _tollFeeSpans));
        return prices.Max();
    }

    internal static int ConvertDateTimeToPrice(DateTime passing, IEnumerable<TollFeeSpan> tollFeeSpans)
    {
        var span = new TimeSpan(0, passing.Hour, passing.Minute, passing.Second, passing.Millisecond);
        return tollFeeSpans.Single(x => x.Start <= span && span < x.End).TollPrice;
    }
}