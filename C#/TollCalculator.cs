using System;
using System.Globalization;
using System.Linq;

namespace TollFeeCalculator;

public class TollCalculator
{
    public const int MAX_DAY_FEE = 60;
    const int FEE_INTERVAL_MINUTES = 60;

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public static int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (dates.Length == 0) { return 0; }

        var orderedDates = dates.OrderBy(d => d).ToArray();

        DateTime intervalStart = orderedDates[0];
        var lastDate = orderedDates.Last();
        int totalFee = 0;
        int tempFee = 0;
        foreach (DateTime date in orderedDates)
        {
            int nextFee = GetTollFee(date, vehicle);

            long diffInMillies = (long)(date - intervalStart).TotalMilliseconds;

            if (diffInMillies >= FEE_INTERVAL_MINUTES * 60 * 1000)
            {
                totalFee += tempFee;
                tempFee = 0;
                intervalStart = date;
            }

            if (nextFee > tempFee)
            {
                tempFee = nextFee;
            }

            if (date == lastDate)
            {
                totalFee += tempFee;
            }
        }
        if (totalFee > MAX_DAY_FEE)
        {
            totalFee = MAX_DAY_FEE;
        }
        return totalFee;
    }

    private static bool IsTollFreeVehicle(Vehicle? vehicle)
    {
        if (vehicle == null) { return false; }
        var vehicleType = vehicle.GetVehicleType();
        return TollFreeVehicles.Contains(vehicleType);
    }

    public static int GetTollFee(DateTime date, Vehicle? vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;
        if (hour < 6) return 0;
        else if (hour == 6 && minute <= 29) return 8;
        else if (hour == 6) return 13;
        else if (hour == 7) return 18;
        else if (hour == 8 && minute <= 29) return 13;
        else if (hour < 15) return 8;
        else if (hour == 15 && minute <= 29) return 13;
        else if (hour < 17) return 18;
        else if (hour < 18) return 13;
        else if (hour == 18 && minute <= 29) return 8;
        else return 0;
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

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

    public enum VehicleType
    {
        Car = 0,
        Motorbike = 1,
        Tractor = 2,
        Emergency = 3,
        Diplomat = 4,
        Foreign = 5,
        Military = 6
    }

    private static readonly List<VehicleType> TollFreeVehicles = [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
        ];
}