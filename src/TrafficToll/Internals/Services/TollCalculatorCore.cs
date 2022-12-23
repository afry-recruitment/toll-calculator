using TrafficToll.Internals.Enums;
using TrafficToll.Internals.ValueObjects;
using static TrafficToll.Internals.Services.StaticTollCalculator;

namespace TrafficToll.Internals.Services;
internal sealed class TollCalculatorCore
{
    private readonly int _maximumDailyFee;
    private readonly TimeSpan _validTollTime;
    private readonly IEnumerable<TollFeeSpan> _tollFeeSpans;
    private readonly IEnumerable<(int year, int month, int day)> _tollFreeDates;
    private readonly IEnumerable<VehicleType> _tollFreeVehicles;

    public TollCalculatorCore(TollCalculationParameters tollCalculationParameters, TollableParameters tollFreeParameters)
    {
        _maximumDailyFee = tollCalculationParameters.MaximumDailyFee;
        _validTollTime = tollCalculationParameters.ValidTollTime;
        _tollFeeSpans = tollCalculationParameters.TollFeeSpans;
        _tollFreeDates = tollFreeParameters.TollFreeDates;
        _tollFreeVehicles = tollFreeParameters.TollFreeVehicles;
    }

    public int CalculateTollFee(IEnumerable<DateTime> passings, int vehicleType)
    {
        if (_tollFreeVehicles.Contains((VehicleType)vehicleType)) return 0;
        var tollablePassings = GetTollablePassings(passings, _tollFreeDates);
        if (!tollablePassings.Any()) return 0;
        var totalSum = CalculateTotalSum(tollablePassings, _validTollTime, _tollFeeSpans);
        return CorrectWithMaximumDailyFee(totalSum, _maximumDailyFee);
    }
}