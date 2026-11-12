using TollCalculator.Models;
using TollCalculator.Services.Policies;

namespace TollCalculator.Services;

public class TollCalculator(
    ITollFeeSchedule feeSchedule,
    ITollFreeVehiclePolicy tollFreeVehiclePolicy,
    ITollFreeDatePolicy tollFreeDatePolicy)
{
    private const int MaxDailyFee = 60;
    private readonly ITollFeeSchedule _feeSchedule = feeSchedule ?? throw new ArgumentNullException(nameof(feeSchedule));
    private readonly ITollFreeVehiclePolicy _tollFreeVehiclePolicy = tollFreeVehiclePolicy ?? throw new ArgumentNullException(nameof(tollFreeVehiclePolicy));
    private readonly ITollFreeDatePolicy _tollFreeDatePolicy = tollFreeDatePolicy ?? throw new ArgumentNullException(nameof(tollFreeDatePolicy));

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        ArgumentNullException.ThrowIfNull(dates);

        if (dates.Length == 0 || _tollFreeVehiclePolicy.IsTollFree(vehicle))
        {
            return 0;
        }

        var orderedDates = dates.OrderBy(d => d).ToArray();

        var intervalStart = orderedDates[0];
        var intervalMaxFee = GetSinglePassFee(intervalStart, vehicle);
        var totalFee = intervalMaxFee;

        for (var i = 1; i < orderedDates.Length; i++)
        {
            var currentDate = orderedDates[i];
            var currentFee = GetSinglePassFee(currentDate, vehicle);

            if ((currentDate - intervalStart).TotalMinutes <= 60)
            {
                if (currentFee > intervalMaxFee)
                {
                    totalFee += currentFee - intervalMaxFee;
                    intervalMaxFee = currentFee;
                }
            }
            else
            {
                intervalStart = currentDate;
                intervalMaxFee = currentFee;
                totalFee += currentFee;
            }

            if (totalFee >= MaxDailyFee)
            {
                return MaxDailyFee;
            }
        }

        return totalFee;
    }

    public int GetSinglePassFee(DateTime date, IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (_tollFreeDatePolicy.IsTollFree(date) || _tollFreeVehiclePolicy.IsTollFree(vehicle))
        {
            return 0;
        }

        return _feeSchedule.GetFee(date);
    }
}