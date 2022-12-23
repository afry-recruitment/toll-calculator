using TrafficToll.Internals.ValueObjects;

namespace TrafficToll.Internals.Services;
internal sealed class TollCalculatorCore
{
    private readonly int _maximumDailyFee;
    private readonly TimeSpan _validTollTime;
    private readonly IEnumerable<TollFeeSpan> _tollFeeSpans;
    private readonly IEnumerable<(int year, int month, int day)> _tollFreeDates;

    public TollCalculatorCore(TollCalculationParameters tollCalculationParameters)
    {
        _maximumDailyFee = tollCalculationParameters.MaximumDailyFee;
        _validTollTime = tollCalculationParameters.ValidTollTime;
        _tollFeeSpans = tollCalculationParameters.TollFeeSpans;
        _tollFreeDates = tollCalculationParameters.TollFreeDates;
    }

    public int CalculateTollFee(IEnumerable<DateTime> passings)
    {
        var tollablePassings = GetTollablePassings(passings);
        if (tollablePassings.Count() == 0) return 0;
        var totalSum = CalculateTotalSum(tollablePassings);
        return CorrectWithMaximumDailyFee(totalSum);
    }

    private int CalculateTotalSum(IEnumerable<DateTime> dateTimes)
    {
        int sum = 0;
        while (true)
        {
            var startTime = dateTimes.First();
            var includedPassings = TakePassingsWithin60MinutesTimeRange(dateTimes, startTime);
            var highestSpanFee = GetHighestFeeFromPassings(includedPassings);
            sum += highestSpanFee;
            dateTimes = dateTimes.Where(x => !includedPassings.Contains(x)).ToArray();

            if (!dateTimes.Any())
                break;
        }

        return sum;
    }

    private int GetHighestFeeFromPassings(IEnumerable<DateTime> includedPassings)
    {
        var maxPrice = 0;
        TimeSpan span;
        foreach (var passing in includedPassings)
        {
            span = new TimeSpan(0, passing.Hour, passing.Minute, passing.Second, passing.Millisecond);
            var price = _tollFeeSpans.Single(x => x.Start <= span && span < x.End).TollPrice;
            maxPrice = price > maxPrice ? price : maxPrice;
        }
        return maxPrice;
    }

    private IEnumerable<DateTime> TakePassingsWithin60MinutesTimeRange(IEnumerable<DateTime> dateTimes, DateTime startTime)
    {
        return dateTimes.Where(x => x >= startTime && x < startTime + _validTollTime);
    }

    private IEnumerable<DateTime> GetTollablePassings(IEnumerable<DateTime> passings)
    {
        var tollablePassings = PassingsWhereThereIsNoHoliday(passings);
        return PassingsWhichAreWeekdays(tollablePassings);
    }

    private static IEnumerable<DateTime> PassingsWhichAreWeekdays(IEnumerable<DateTime> passings)
    {
        return passings.Where(x => !(x.DayOfWeek == DayOfWeek.Saturday || x.DayOfWeek == DayOfWeek.Sunday));
    }

    private IEnumerable<DateTime> PassingsWhereThereIsNoHoliday(IEnumerable<DateTime> passings)
    {
        return passings.Where(x => !_tollFreeDates.Contains((x.Year, x.Month, x.Day)));
    }

    private int CorrectWithMaximumDailyFee(int totalSum)
    {
        return totalSum < _maximumDailyFee ? totalSum : _maximumDailyFee;
    }
}