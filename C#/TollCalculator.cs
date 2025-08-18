using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator;
using PublicHoliday;

public class TollCalculator
{
    /// <summary>
    /// Calculates the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="dates">Date and time of all passes on one day.</param>
    /// <returns>the total toll fee for that day</returns>
    public int GetDailyTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (dates == null || dates.Length == 0) return 0;

        Array.Sort(dates);
        int totalFee = 0;
        List<DateTime> withinAnHour = new List<DateTime>();

        foreach (DateTime date in dates)
        {
            if (withinAnHour.Count == 0)
            {
                withinAnHour.Add(date);
                continue;
            }

            if ((date - withinAnHour[0]).TotalMinutes < 60)
            {
                withinAnHour.Add(date);
            }
            else
            {
                totalFee += withinAnHour.Max(d => GetSingleTollFee(vehicle, d));

                withinAnHour.Clear();
                withinAnHour.Add(date);
            }
        }

        if (withinAnHour.Count > 0)
        {
            totalFee += withinAnHour
                .Max(d => GetSingleTollFee(vehicle, d));
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null)
        {
            //Handle vehicle being null.
            Console.WriteLine("Vehicle is null");
            return false;
        }
        string vehicleType = vehicle.GetVehicleType();
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
    }

    public int GetSingleTollFee(IVehicle vehicle, DateTime date)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        var time = date.TimeOfDay;

        if (time >= TimeSpan.FromHours(6) && time < TimeSpan.FromHours(6.5)) return 8;    // 06:00–06:29
        if (time >= TimeSpan.FromHours(6.5) && time < TimeSpan.FromHours(7)) return 13;   // 06:30–06:59
        if (time >= TimeSpan.FromHours(7) && time < TimeSpan.FromHours(8)) return 18;     // 07:00–07:59
        if (time >= TimeSpan.FromHours(8) && time < TimeSpan.FromHours(8.5)) return 13;   // 08:00–08:29
        if (time >= TimeSpan.FromHours(8.5) && time < TimeSpan.FromHours(15)) return 8;   // 08:30–14:59
        if (time >= TimeSpan.FromHours(15) && time < TimeSpan.FromHours(15.5)) return 13; // 15:00–15:29
        if (time >= TimeSpan.FromHours(15.5) && time < TimeSpan.FromHours(17)) return 18; // 15:30–16:59
        if (time >= TimeSpan.FromHours(17) && time < TimeSpan.FromHours(18)) return 13;   // 17:00–17:59
        if (time >= TimeSpan.FromHours(18) && time < TimeSpan.FromHours(18.5)) return 8;  // 18:00–18:29

        return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        int month = date.Month;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (new SwedenPublicHoliday().IsPublicHoliday(date) || month == 7)
        {
            return true;
        }
        return false;
    }
}