using TrafficToll.Internals.ValueObjects;
using static TrafficToll.Internals.Services.StaticTollCalculator;

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
        var totalSum = CalculateTotalSum(passings, _validTollTime, _tollFeeSpans);
        return CorrectWithMaximumDailyFee(totalSum, _maximumDailyFee);
    }
}