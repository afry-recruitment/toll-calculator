using System;
using System.Globalization;

using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public class TollCalculator
{
    private const int MaxDailyFee = 60;
    private static readonly TimeSpan ChargingWindow = TimeSpan.FromMinutes(60);

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        int totalFee = 0;
        int windowFee = 0;
        DateTime? windowStart = null;

        foreach (DateTime date in dates)
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