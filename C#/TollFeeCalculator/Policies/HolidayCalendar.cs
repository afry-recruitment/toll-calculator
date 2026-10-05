namespace TollFeeCalculator.Policies;

public class HolidayCalendar : IHolidayCalendar
{
    public bool IsTollFree(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return true;

        if (date.Year != 2013)
            return false;

        int month = date.Month;
        int day = date.Day;
        return month == 1 && day == 1 ||
               month == 3 && (day == 28 || day == 29) ||
               month == 4 && (day == 1 || day == 30) ||
               month == 5 && (day == 1 || day == 8 || day == 9) ||
               month == 6 && (day == 5 || day == 6 || day == 21) ||
               month == 7 ||
               month == 11 && day == 1 ||
               month == 12 && (day == 24 || day == 25 || day == 26 || day == 31);
    }
}
