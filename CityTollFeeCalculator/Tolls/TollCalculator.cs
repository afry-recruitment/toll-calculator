using CityTollFeeCalculator.Tolls.Policy;
using CityTollFeeCalculator.Vehicle;

namespace CityTollFeeCalculator.Tolls;

public class TollCalculator(ITollFeePolicy policy) : ITollCalculator
{
    public int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> dates)
    {
        if (policy.IsTollFreeVehicle(vehicle) || dates is null || !dates.Any()) return 0;
        int totalFee = 0;
        DateTime? previousTollTime = null;
        int previousFee = 0;

        foreach (var datetime in dates)
        {
            if (policy.IsTollFreeDate(DateOnly.FromDateTime(datetime))) continue;

            var currentFee = GetTollFee(datetime);
            if (!previousTollTime.HasValue || !previousTollTime.Value.IsWithinOneHour(datetime))
            {
                totalFee += currentFee;
            }
            else
            {
                if (currentFee > previousFee)
                {
                    totalFee += currentFee - previousFee;
                }
            }
            previousFee = currentFee;
            previousTollTime = datetime;
        }
        return Math.Min(totalFee, policy.MAX_FEE_PER_DAY);

        int GetTollFee(DateTime date)
        {
            var time = date.TimeOfDay;
            return TollRates.Rates
                .Where(rate => time >= rate.Start && time <= rate.End)
                .Select(rate => rate.Fee)
                .DefaultIfEmpty(0)
                .First();
        }
    }
}
