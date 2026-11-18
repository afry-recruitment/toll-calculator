using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public class TollCalculator
{
    private const int DefaultMaxDailyFee = 60;
    private static readonly TimeSpan ChargingWindow = TimeSpan.FromMinutes(60);

    private readonly FeeSchedule _feeSchedule;
    private readonly int _maxDailyFee;

    /// <summary>
    /// Creates a calculator with the city's fee schedule and daily maximum.
    /// </summary>
    public TollCalculator()
        : this(FeeSchedule.Default, DefaultMaxDailyFee)
    {
    }

    public TollCalculator(FeeSchedule feeSchedule, int maxDailyFee)
    {
        ArgumentNullException.ThrowIfNull(feeSchedule);
        ArgumentOutOfRangeException.ThrowIfNegative(maxDailyFee);

        _feeSchedule = feeSchedule;
        _maxDailyFee = maxDailyFee;
    }

    /// <summary>
    /// Calculates the total toll fee for a vehicle's passages.
    /// Each day is calculated on its own and has its own daily maximum.
    /// </summary>
    /// <param name="vehicle">The vehicle. A missing vehicle is charged as a paying vehicle.</param>
    /// <param name="dates">Date and time of the passages in local time, in any order.</param>
    /// <returns>The total toll fee.</returns>
    public int GetTollFee(IVehicle? vehicle, DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(dates);

        return dates
            .GroupBy(date => date.Date)
            .Sum(day => GetDailyFee(vehicle, day));
    }

    private int GetDailyFee(IVehicle? vehicle, IEnumerable<DateTime> dates)
    {
        int totalFee = 0;
        int windowFee = 0;
        DateTime? windowStart = null;

        foreach (DateTime date in dates.Order())
        {
            int fee = GetTollFee(date, vehicle);
            if (fee == 0) continue;

            bool startsNewWindow = windowStart is null || date - windowStart > ChargingWindow;
            if (startsNewWindow)
            {
                totalFee += windowFee;
                windowStart = date;
                windowFee = fee;
            }
            else
            {
                windowFee = Math.Max(windowFee, fee);
            }
        }

        totalFee += windowFee;

        return Math.Min(totalFee, _maxDailyFee);
    }

    /// <summary>
    /// Calculates the toll fee for a single passage.
    /// </summary>
    /// <param name="date">Date and time of the passage in local time.</param>
    /// <param name="vehicle">The vehicle. A missing vehicle is charged as a paying vehicle.</param>
    /// <returns>The toll fee for the passage.</returns>
    public int GetTollFee(DateTime date, IVehicle? vehicle)
    {
        if (TollFreeDates.Includes(date) || TollFreeVehicles.Includes(vehicle)) return 0;

        return _feeSchedule.FeeAt(TimeOnly.FromDateTime(date));
    }
}
