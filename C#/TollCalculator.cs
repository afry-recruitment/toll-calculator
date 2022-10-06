using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{


    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            //updated minutes by using Timespan struct.
            TimeSpan minutes = date - intervalStart;

            if (minutes.TotalMinutes <= 60)
            {
                if (totalFee > 0)
                {
                    totalFee -= tempFee;
                }
                if (nextFee >= tempFee)
                {
                    tempFee = nextFee;
                }
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;

                intervalStart = date;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        // It returns true if VehicleType name or given integral value exists in enum.
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        //I Added if sats for other times during a day.

        if (hour < 6) return 8;
        else if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 8 && minute >= 30 && minute <= 59) return 8;
        else if (hour >= 9 && hour <= 14) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else if (hour > 18) return 8;
        else return 0;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        //It returns true if date is weekend or holiday
        return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || IsHoliday(date)) ? true : false;
    }


    private Boolean IsHoliday(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (year == 2022)
        {
            if (month == 1 && day == 6 ||
                month == 4 && (day == 15 || day == 17 || day == 18) ||
                month == 5 && (day == 1 || day == 26) ||
                month == 6 && (day == 5 || day == 6 || day == 25) ||
                month == 11 && day == 5 ||
                month == 12 && (day == 25 || day == 26))
            {
                return true;
            }
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