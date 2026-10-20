using System;
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

    public int GetTollFee(VehicleType vehicle, DateTime[] dates)
    {
        bool isTollFreeVehicle = IsTollFreeVehicle(vehicle);
        ArgumentNullException.ThrowIfNull(dates);

        if (dates.Length == 0) return 0;

        foreach (DateTime date in dates)
        {
            ValidateYear(date, nameof(dates));
        }

        if (dates.Any(date => date.Date != dates[0].Date))
        {
            throw new ArgumentException("All passages must be on the same calendar date.", nameof(dates));
        }

        if (isTollFreeVehicle) return 0;

        var sortedDates = dates.OrderBy(date => date).ToArray();

        DateTime intervalStart = sortedDates[0];
        int totalFee = 0;
        int highestFeeInInterval = 0;

        foreach (DateTime date in sortedDates)
        {
            int nextFee = GetTollFee(date, vehicle);

            double minutes = (date - intervalStart).TotalMinutes;

            if (minutes <= 60)
            {
                highestFeeInInterval = Math.Max(highestFeeInInterval, nextFee);
            }
            else
            {
                totalFee += highestFeeInInterval;

                intervalStart = date;
                highestFeeInInterval = nextFee;
            }
        }

        totalFee += highestFeeInInterval;
        return Math.Min(totalFee, 60);
    }

    private static bool IsTollFreeVehicle(VehicleType vehicle)
    {
        switch (vehicle)
        {
            case VehicleType.Car:
                return false;
            case VehicleType.Motorbike:
            case VehicleType.Tractor:
            case VehicleType.Emergency:
            case VehicleType.Diplomat:
            case VehicleType.Foreign:
            case VehicleType.Military:
                return true;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(vehicle), vehicle, "A supported vehicle type must be specified.");
        }
    }

    public int GetTollFee(DateTime date, VehicleType vehicle)
    {
        bool isTollFreeVehicle = IsTollFreeVehicle(vehicle);
        ValidateYear(date, nameof(date));

        if (IsTollFreeDate(date) || isTollFreeVehicle) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;                        // 06:00-06:29
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;                 // 06:30-06:59
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;                  // 07:00-07:59
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;                  // 08:00-08:29
        else if ((hour == 8 && minute >= 30) || (hour >= 9 && hour <= 14)) return 8;   // 08:30-14:59
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;                 // 15:00-15:29
        else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return 18;  // 15:30-16:59
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;                 // 17:00-17:59
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;                  // 18:00-18:29
        else return 0;
    }

    private static void ValidateYear(DateTime date, string parameterName)
    {
        if (date.Year != 2013)
        {
            throw new ArgumentOutOfRangeException(parameterName, date, "Only dates in 2013 are supported.");
        }
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

}
