public static class TollDateHelper
{
    public static bool IsTollFreeDate(DateTime date)
    {
        return IsWeekend(date) || IsHoliday(date);
    }

    private static bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday ||
               date.DayOfWeek == DayOfWeek.Sunday;
    }

    private static bool IsHoliday(DateTime date)
    {
        var holidays = new List<DateTime>
        {
            new DateTime(date.Year, 1, 1),
            new DateTime(date.Year, 12, 25),
            new DateTime(date.Year, 12, 26),
        };

        return holidays.Any(d => d.Date == date.Date);
    }
}