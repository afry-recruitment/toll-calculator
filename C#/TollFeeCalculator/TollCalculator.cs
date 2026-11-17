using System;
using System.Globalization;

using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public class TollCalculator
{
    private const int MaxDailyFee = 60;
    private static readonly TimeSpan ChargingWindow = TimeSpan.FromMinutes(60);

    /// <summary>
    /// Calculates the total toll fee for a vehicle's passages.
    /// Each day is calculated on its own and has its own daily maximum.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="dates">Date and time of the passages in local time, in any order.</param>
    /// <returns>The total toll fee.</returns>
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(dates);

        return dates
            .GroupBy(date => date.Date)
            .Sum(day => GetDailyFee(vehicle, day));
    }

    private int GetDailyFee(IVehicle vehicle, IEnumerable<DateTime> dates)
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

        return Math.Min(totalFee, MaxDailyFee);
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (TollFreeDates.Includes(date) || TollFreeVehicles.Includes(vehicle)) return 0;

        return FeeSchedule.FeeAt(TimeOnly.FromDateTime(date));
    }
}