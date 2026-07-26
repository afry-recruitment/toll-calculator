using System;
using System.Globalization;

namespace TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        return vehicle.IsTollFree();
    }

        private bool IsTollFreeDate(DateTime date)
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
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (dates.Length == 0) return 0; 

        DateTime intervalStart = dates[0];
        int intervalFee = 0;
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);

            if ((date - intervalStart).TotalMinutes <= 60)
            {
                if (nextFee > intervalFee) intervalFee = nextFee;  
            }
            else
            {
                totalFee += intervalFee;   
                intervalStart = date;      
                intervalFee = nextFee;
            }
        }
        totalFee += intervalFee;           

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute <= 29) return 8;
        else if (hour == 6) return 13;
        else if (hour == 7) return 18;
        else if (hour == 8 && minute <= 29) return 13;
        else if (hour == 8 && minute >= 30) return 8;
        else if (hour >= 9 && hour <= 14) return 8;
        else if (hour == 15 && minute <= 29) return 13;
        else if (hour == 15) return 18;
        else if (hour == 16) return 18;
        else if (hour == 17) return 13;
        else if (hour == 18 && minute <= 29) return 8;
        else return 0;
    }

}