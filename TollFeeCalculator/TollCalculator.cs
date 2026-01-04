using System;
using System.Globalization;
using PublicHoliday;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    private const int MaxDailyFee = 60;
    private const int MaxIntervalMinutes = 60;

    public int GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
        if (dates == null) throw new ArgumentNullException(nameof(dates), "Dates array cannot be null.");

        try
        {
            var passagesByDay = dates.GroupBy(d => d.Date);
            int totalFeeForAllDays = 0;

            foreach (var dayGroup in passagesByDay)
            {
                totalFeeForAllDays += CalculateDailyFee(vehicle, dayGroup.ToArray());
            }

            return totalFeeForAllDays;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error calculating toll fee. Ensure date formats are correct.", ex);
        }
    }

    private int CalculateDailyFee(Vehicle vehicle, DateTime[] dates)
    {
        var sortedDates = dates.OrderBy(d => d).ToArray();

        DateTime intervalStart = sortedDates[0];

        int totalFee = 0;
        int maxFeeInInterval = GetTollFee(intervalStart, vehicle);

        foreach (DateTime date in sortedDates)
        {
            int currentFee = GetTollFee(date, vehicle);
            double minutes = (date - intervalStart).TotalMinutes;

            if (minutes <= MaxIntervalMinutes)
            {
                if (currentFee > maxFeeInInterval) maxFeeInInterval = currentFee;
            }
            else
            {
                totalFee += maxFeeInInterval;
                intervalStart = date;
                maxFeeInInterval = currentFee;
            }
        }

        totalFee += maxFeeInInterval;

        if (totalFee > MaxDailyFee)
        {
            totalFee = MaxDailyFee;
        }

        return totalFee;
    }


    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;

        return vehicle.Type switch
        {
            VehicleType.Motorbike => true,
            VehicleType.Tractor => true,
            VehicleType.Emergency => true,
            VehicleType.Diplomat => true,
            VehicleType.Foreign => true,
            VehicleType.Military => true,
            VehicleType.Car => false,
            _ => throw new ArgumentOutOfRangeException(nameof(vehicle.Type), "Unsupported vehicle type.")
        };
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        TimeSpan time = date.TimeOfDay;

        if (IsBetween(time, 6, 0, 6, 29)) return 8;
        if (IsBetween(time, 6, 30, 6, 59)) return 13;
        if (IsBetween(time, 7, 0, 7, 59)) return 18;
        if (IsBetween(time, 8, 0, 8, 29)) return 13;
        if (IsBetween(time, 8, 30, 14, 59)) return 8;
        if (IsBetween(time, 15, 0, 15, 29)) return 13;
        if (IsBetween(time, 15, 30, 16, 59)) return 18;
        if (IsBetween(time, 17, 0, 17, 59)) return 13;
        if (IsBetween(time, 18, 0, 18, 29)) return 8;

        return 0;
    }

    private bool IsBetween(TimeSpan time, int startH, int startM, int endH, int endM)
    {
        var start = new TimeSpan(startH, startM, 0);
        var end = new TimeSpan(endH, endM, 59);
        return time >= start && time <= end;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
        if (date.Month == 7) return true;

        var swedishHolidays = new SwedenPublicHoliday();

        if (swedishHolidays.IsPublicHoliday(date)) return true;

        return false;
    }
}