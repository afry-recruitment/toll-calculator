namespace TollFeeCalculator;
public static class TollFreeHelper
{

    /// <summary>
    /// Check If Vehicle Type is Toll Free
    /// </summary>
    /// <param name="vehicle"></param>
    /// <returns></returns>
    public static bool IsTollFreeVehicle(IVehicle vehicle)
    {
        return vehicle?.IsTollFree ?? false;
    }

    /// <summary>
    /// if Holiday list is passed, Calculate If the day is Toll Free
    /// Else check the annual dates to find Yearly Holiday
    /// </summary>
    /// <returns>If date is toll free</returns>
    /// <param name="date">The date, must be of DateTimeKind Local.</param>
    public static bool IsTollFreeDate(DateTime date, DateOnly[] _holidayList)
    {
        var month = date.Month;
        var day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (_holidayList.Count() > 1)
        {
            return (_holidayList.Count(d => d == DateOnly.FromDateTime(date)) > 0);
        }
        return (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31));
    }
}