using TollFeeCalculator.Exceptions;
using TollFeeCalculator.Models;
using TollFeeCalculator.Policies;

namespace TollFeeCalculator;

public interface ICalculateTollFeeService
{
    TollFeeCalculationResult CalculateTotalTollFee(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes);
}

public class CalculateTollFeeService(ITollFeePolicy policy) : ICalculateTollFeeService
{
    private readonly ITollFeePolicy _policy = policy ?? throw new ArgumentNullException(nameof(policy));

    public TollFeeCalculationResult CalculateTotalTollFee(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes)
    {
        if (passageTimes == null)
            throw new ArgumentNullException(nameof(passageTimes));

        var currency = _policy.Currency;
        var dailyMaxFee = _policy.DailyMaxFee;

        if (passageTimes.Length == 0 || _policy.IsTollFreeDate(date) || _policy.IsTollFreeVehicleType(vehicleType))
            return new TollFeeCalculationResult(date, 0, currency);

        var harmonizedPassageTimes = passageTimes.Select(time => new TimeOnly(time.Hour, time.Minute));

        decimal totalFee = 0;
        foreach (var passageTime in harmonizedPassageTimes.GroupBy(time => time.Hour))
        {
            if (passageTime.Count() == 1)
            {
                totalFee += GetTollFee(passageTime.First());
                continue;
            }

            totalFee += passageTime.Max(GetTollFee);
        }

        return totalFee > dailyMaxFee ? new TollFeeCalculationResult(date, dailyMaxFee, currency) : new TollFeeCalculationResult(date, totalFee, currency);
    }

    private decimal GetTollFee(TimeOnly passageTime)
    {
        var rate = _policy.TollFeeRates.FirstOrDefault(rate => rate.IsWithinTimeInterval(passageTime));

        if (rate == null)
            throw new MissingTollFeeRateException(passageTime);

        return rate.Fee;
    }
}