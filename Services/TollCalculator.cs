using tollFeeCalculator.Models;

namespace tollFeeCalculator.Services;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetFeeForHour(date, vehicle);
            int tempFee = GetFeeForHour(intervalStart, vehicle);

            var diffInMinutes = (date - intervalStart).TotalMinutes;

            if (diffInMinutes < 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        String vehicleType = vehicle.GetVehicleType();

        bool isTollFree = Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
        return isTollFree;
    }

    public int GetFeeForHour(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if ((hour >= 8 && hour <= 14) && (minute >= 00 && minute <= 59)) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 16 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

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

        return false;
    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}