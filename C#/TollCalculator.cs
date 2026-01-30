using System;
using System.Collections.Generic;
using System.Globalization;
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

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (dates == null || dates.Length == 0) return 0;

        Array.Sort(dates);

        DateTime firstDay = dates[0].Date;
        foreach (DateTime date in dates)
        {
            if (date.Date != firstDay)
            {
                throw new ArgumentException("All dates must be on the same day");
            }
        }

        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int intervalFee = GetTollFee(intervalStart, vehicle);
        totalFee += intervalFee;

        for (int i = 1; i < dates.Length; i++)
        {
            DateTime date = dates[i];
            int nextFee = GetTollFee(date, vehicle);
            double minutes = (date - intervalStart).TotalMinutes;

            if (minutes <= 60)
            {
                if (nextFee > intervalFee)
                {
                    totalFee += nextFee - intervalFee;
                    intervalFee = nextFee;
                }
            }
            else
            {
                intervalStart = date;
                intervalFee = nextFee;
                totalFee += nextFee;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 8 && minute >= 30) return 8;
        else if (hour >= 9 && hour <= 14) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 30) return 18;
        else if (hour == 16) return 18;
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

        DateTime localDate = date.Date;
        return GetTollFreeDates(year).Contains(localDate);
    }

    private HashSet<DateTime> GetTollFreeDates(int year)
    {
        HashSet<DateTime> dates = new HashSet<DateTime>();

        dates.Add(new DateTime(year, 1, 1));
        dates.Add(new DateTime(year, 1, 6));
        dates.Add(new DateTime(year, 5, 1));
        dates.Add(new DateTime(year, 6, 6));
        dates.Add(new DateTime(year, 12, 24));
        dates.Add(new DateTime(year, 12, 25));
        dates.Add(new DateTime(year, 12, 26));
        dates.Add(new DateTime(year, 12, 31));

        DateTime easterSunday = GetEasterSunday(year);
        dates.Add(easterSunday.AddDays(-2)); // Good Friday
        dates.Add(easterSunday); // Easter Sunday
        dates.Add(easterSunday.AddDays(1)); // Easter Monday
        dates.Add(easterSunday.AddDays(39)); // Ascension Day
        dates.Add(easterSunday.AddDays(49)); // Pentecost Sunday

        DateTime midsummerEve = GetMidsummerEve(year);
        dates.Add(midsummerEve);
        dates.Add(midsummerEve.AddDays(1)); // Midsummer Day

        int julyDays = DateTime.DaysInMonth(year, 7);
        for (int d = 1; d <= julyDays; d++)
        {
            dates.Add(new DateTime(year, 7, d));
        }

        return dates;
    }

    private DateTime GetMidsummerEve(int year)
    {
        DateTime june19 = new DateTime(year, 6, 19);
        int daysUntilFriday = ((int)DayOfWeek.Friday - (int)june19.DayOfWeek + 7) % 7;
        return june19.AddDays(daysUntilFriday);
    }

    private DateTime GetEasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31; // 3=March, 4=April
        int day = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateTime(year, month, day);
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
