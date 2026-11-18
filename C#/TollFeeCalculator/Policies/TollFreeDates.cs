using System.Collections.Concurrent;

namespace TollFeeCalculator.Policies;

// Weekends, public holidays, the day before a public holiday and all of July are toll-free.
public static class TollFreeDates
{
    // The holidays of a year never change, so they are calculated once and kept.
    // The dictionary is thread-safe because the class is shared by all callers.
    private static readonly ConcurrentDictionary<int, HashSet<DateOnly>> HolidaysByYear = new();

    public static bool Includes(DateTime date)
    {
        var day = DateOnly.FromDateTime(date);

        return IsWeekend(day)
            || day.Month == 7
            || IsPublicHoliday(day)
            || IsDayBeforePublicHoliday(day);
    }

    private static bool IsDayBeforePublicHoliday(DateOnly day)
    {
        // The last day of the calendar has no next day to look up, but it is a New Year's Eve like any other.
        if (day == DateOnly.MaxValue) return true;

        return IsPublicHoliday(day.AddDays(1));
    }

    private static bool IsWeekend(DateOnly day)
    {
        return day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    private static bool IsPublicHoliday(DateOnly day)
    {
        return HolidaysByYear.GetOrAdd(day.Year, PublicHolidays).Contains(day);
    }

    // The Swedish public holidays of a year, according to the current rules.
    private static HashSet<DateOnly> PublicHolidays(int year)
    {
        var easterSunday = EasterSunday(year);

        return
        [
            new DateOnly(year, 1, 1), // New Year's Day
            new DateOnly(year, 1, 6), // Epiphany
            easterSunday.AddDays(-2), // Good Friday
            easterSunday,
            easterSunday.AddDays(1), // Easter Monday
            new DateOnly(year, 5, 1), // May Day
            easterSunday.AddDays(39), // Ascension Day
            easterSunday.AddDays(49), // Whit Sunday
            new DateOnly(year, 6, 6), // National Day
            FirstSaturdayFrom(new DateOnly(year, 6, 20)), // Midsummer Day
            FirstSaturdayFrom(new DateOnly(year, 10, 31)), // All Saints' Day
            new DateOnly(year, 12, 25), // Christmas Day
            new DateOnly(year, 12, 26), // Boxing Day
        ];
    }

    private static DateOnly FirstSaturdayFrom(DateOnly day)
    {
        while (day.DayOfWeek != DayOfWeek.Saturday)
        {
            day = day.AddDays(1);
        }

        return day;
    }

    // The anonymous Gregorian algorithm (Meeus/Jones/Butcher) for the date of Easter Sunday.
    private static DateOnly EasterSunday(int year)
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
        int month = (h + l - 7 * m + 114) / 31;
        int day = (h + l - 7 * m + 114) % 31 + 1;

        return new DateOnly(year, month, day);
    }
}
