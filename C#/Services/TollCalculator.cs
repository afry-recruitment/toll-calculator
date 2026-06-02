using System;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services;

/// <summary>
/// Calculates the daily congestion tax for a vehicle.
/// </summary>
/// <remarks>
/// All <see cref="DateTime"/> values are interpreted as Swedish local time
/// (Europe/Stockholm), since the congestion tax is defined in local time.
/// The caller is responsible for converting any UTC or other time-zone values
/// before passing them in.
/// </remarks>
public class TollCalculator
{
    private const int MaxDailyFee = 60;
    private const int ChargeWindowMinutes = 60;

    /// <summary>
    /// Calculate the total toll fee for one day.
    /// Dates must all belong to the same day. Unsorted input is handled internally.
    /// </summary>
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
        if (dates == null || dates.Length == 0) return 0;

        // A toll-free vehicle is never charged, so skip all per-passage work.
        if (IsTollFreeVehicle(vehicle)) return 0;

        DateTime[] sorted = (DateTime[])dates.Clone();
        Array.Sort(sorted);

        if (sorted[0].Year < 2013)
            throw new ArgumentException("Congestion tax was not applicable before 2013.", nameof(dates));

        if (sorted[sorted.Length - 1].Date != sorted[0].Date)
            throw new ArgumentException("All dates must belong to the same day.", nameof(dates));

        DateTime intervalStart = sorted[0];
        int intervalMaxFee = 0;
        int totalFee = 0;

        foreach (DateTime date in sorted)
        {
            int fee = GetFeeAtTime(date);
            double minutes = (date - intervalStart).TotalMinutes;

            if (minutes <= ChargeWindowMinutes)
            {
                if (fee > intervalMaxFee) intervalMaxFee = fee;
            }
            else
            {
                totalFee += intervalMaxFee;
                intervalStart = date;
                intervalMaxFee = fee;
            }
        }

        totalFee += intervalMaxFee;
        return Math.Min(totalFee, MaxDailyFee);
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        return vehicle.IsTollFree;
    }

    private int GetFeeAtTime(DateTime date)
    {
        if (IsTollFreeDate(date)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute < 30) return 8;
        if (hour == 6) return 13;
        if (hour == 7) return 18;
        if (hour == 8 && minute < 30) return 13;
        if (hour >= 8 && hour < 15) return 8;
        if (hour == 15 && minute < 30) return 13;
        if (hour == 15 || hour == 16) return 18;
        if (hour == 17) return 13;
        if (hour == 18 && minute < 30) return 8;
        return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        if (date.Month == 7)
            return true;

        // A public holiday, or the day before one, is toll-free.
        return IsPublicHoliday(date) || IsPublicHoliday(date.AddDays(1));
    }

    /// <summary>Swedish public holidays ("röda dagar") relevant to the congestion tax.</summary>
    private static bool IsPublicHoliday(DateTime date)
    {
        int month = date.Month;
        int day = date.Day;

        // Fixed-date holidays.
        if ((month == 1 && day == 1) ||    // Nyårsdagen
            (month == 1 && day == 6) ||    // Trettondedag jul
            (month == 5 && day == 1) ||    // Första maj
            (month == 6 && day == 6) ||    // Nationaldagen
            (month == 12 && day == 25) ||  // Juldagen
            (month == 12 && day == 26))    // Annandag jul
            return true;

        // Easter-based movable holidays.
        DateTime easter = EasterSunday(date.Year);
        DateTime d = date.Date;
        if (d == easter.AddDays(-2) ||  // Långfredag
            d == easter ||              // Påskdagen
            d == easter.AddDays(1) ||   // Annandag påsk
            d == easter.AddDays(39) ||  // Kristi himmelsfärds dag
            d == easter.AddDays(49))    // Pingstdagen
            return true;

        // Midsommardagen: the Saturday that falls 20–26 June.
        if (month == 6 && day >= 20 && day <= 26 && date.DayOfWeek == DayOfWeek.Saturday)
            return true;

        // Alla helgons dag: the Saturday that falls 31 Oct – 6 Nov.
        if (((month == 10 && day == 31) || (month == 11 && day <= 6)) &&
            date.DayOfWeek == DayOfWeek.Saturday)
            return true;

        return false;
    }

    /// <summary>Gregorian Easter Sunday (Anonymous Gregorian / Meeus–Jones–Butcher algorithm).</summary>
    private static DateTime EasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int dd = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - dd - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateTime(year, month, day);
    }
}