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
        var totalSum = CalculateTotalSum(passings);
        return CorrectWithMaximumDailyFee(totalSum, _maximumDailyFee);
    }

    private int CalculateTotalSum(IEnumerable<DateTime> passings)
    {
        int sum = 0;
        while (true)
        {
            var includedPassings = TakePassingsWithin1HourTimeRange(passings, _validTollTime);
            var highestSpanFee = GetHighestFeeFromPassings(includedPassings, _tollFeeSpans);
            sum += highestSpanFee;
            passings = passings.Where(x => !includedPassings.Contains(x)).ToArray();

            if (!passings.Any())
                break;
        }

        return sum;
    }

    private static int GetHighestFeeFromPassings(IEnumerable<DateTime> includedPassings, IEnumerable<TollFeeSpan> tollFeeSpans)
    {
        var maxPrice = 0;
        TimeSpan span;
        foreach (var passing in includedPassings)
        {
            span = new TimeSpan(0, passing.Hour, passing.Minute, passing.Second, passing.Millisecond);
            var price = tollFeeSpans.Single(x => x.Start <= span && span < x.End).TollPrice;
            maxPrice = price > maxPrice ? price : maxPrice;
        }
        return maxPrice;
    }

    private static IEnumerable<DateTime> TakePassingsWithin1HourTimeRange(
        IEnumerable<DateTime> dateTimes,
        TimeSpan validTollTime)
    {
        var startTime = dateTimes.First();
        return dateTimes.Where(x => x >= startTime && x < startTime + validTollTime);
    }

    private static int CorrectWithMaximumDailyFee(int totalSum, int maximumDailyFee)
    {
        return totalSum < maximumDailyFee ? totalSum : maximumDailyFee;
    }
}