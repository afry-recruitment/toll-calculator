namespace TollFeeCalculator;
public static class TollCalculationHelper
{
    /// <summary>
    /// Calculate Toll Fee Based On Hour
    /// </summary>
    /// <returns>The toll fee at given hour.</returns>
    /// <param name="date">The date, must be of DateTimeKind Local.</param>
    public static int TollChargesPerPass(DateTime date)
    {
        int hour = date.Hour;
        int minute = date.Minute;

        if (MinimumFeeHours(hour, minute))
            return 8;
        else if (AverageFeeHours(hour, minute))
            return 13;
        else if (RushHours(hour, minute))
            return 18;
        else
            return 0;
    }

    /// <summary>
    /// Calculate Toll Free Dates
    /// </summary>
    /// <returns>If date is toll free</returns>
    /// <param name="date">The date, must be of DateTimeKind Local.</param>
    public static bool IsTollFreeDate(DateTime date, DateOnly[] _holidayList)
    {
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (_holidayList.Count() > 1)
        {
            var datePart = DateOnly.FromDateTime(date);
            if (_holidayList.Count(d => d == datePart) > 0)
                return true;
        }
        else if (month == 1 && day == 1 ||
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

    public static bool IsTollFreeVehicle(IVehicle vehicle)
    {
        return vehicle?.IsTollFree ?? false;
    }

    /// <summary>
    /// Rush Hour Timings
    /// </summary>
    /// <returns>If Given Time Is Rush Hour</returns>
    /// <param name="hour">Given Hour</param>
    /// <param name="minute">Given Minute</param>
    private static bool RushHours(int hour, int minute)
    {
        if (hour == 7 || (hour == 15 && minute >= 30) || hour == 16) return true;
        return false;
    }

    /// <summary>
    /// Minimum Fee Timings
    /// </summary>
    /// <returns>If Given Time Is Minimun Fee Hour</returns>
    /// <param name="hour">Given Hour</param>
    /// <param name="minute">Given Minute</param>
    private static bool MinimumFeeHours(int hour, int minute)
    {
        if ((hour == 6 && minute >= 0 && minute <= 29)
        || (hour == 8 && minute <= 30)
        || (hour >= 9 && hour <= 14)
        || (hour == 18 && minute >= 0 && minute <= 29)) return true;
        return false;
    }

    /// <summary>
    /// Average Fee Timings
    /// </summary>
    /// <returns>If Given Time Is Average Fee Hour</returns>
    /// <param name="hour">Given Hour</param>
    /// <param name="minute">Given Minute</param>
    private static bool AverageFeeHours(int hour, int minute)
    {
        if ((hour == 6 && minute >= 30 && minute <= 59)
        || (hour == 8 && minute >= 0 && minute <= 29)
        || (hour == 15 && minute >= 0 && minute <= 29)
        || hour == 17) return true;
        return false;
    }
}
