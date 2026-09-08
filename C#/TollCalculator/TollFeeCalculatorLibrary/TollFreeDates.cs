using System.Collections.Frozen;

namespace TollFeeCalculatorLibrary;

internal class TollFreeDates
{
    readonly FrozenSet<DateOnly> allHolidays;

    public TollFreeDates()
    {
        DateTime started = DateTime.UtcNow.AddYears(-1);

        allHolidays = PrecomputeHolidays(started.Year, started.AddYears(10).Year);
    }

    public bool IsTollFreeDate(DateOnly date)
    {
        return IsWeekend(date) || IsPublicHoliday(date);
    }

    static bool IsWeekend(DateOnly date)
    {
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    bool IsPublicHoliday(DateOnly date)
    {
        return allHolidays.Contains(date);
    }

    static FrozenSet<DateOnly> PrecomputeHolidays(int startYear, int endYear)
    {
        var holidays = new HashSet<DateOnly>();

        for (int year = startYear; year <= endYear; year++)
        {
            // Add Fixed Swedish Holidays
            holidays.Add(new DateOnly(year, 1, 1));   // Nyårsdagen
            holidays.Add(new DateOnly(year, 1, 6));   // Trettondedag jul
            holidays.Add(new DateOnly(year, 5, 1));   // Första maj
            holidays.Add(new DateOnly(year, 6, 6));   // Nationaldagen
            holidays.Add(new DateOnly(year, 12, 25)); // Juldagen
            holidays.Add(new DateOnly(year, 12, 26)); // Annandag jul

            // Add Moving Holidays (Easter-based)
            DateOnly easter = GetEasterSunday(year);
            holidays.Add(easter.AddDays(39)); // Kristi himmelsfärd (Ascension)
            holidays.Add(easter.AddDays(50)); // Annandag Pingst
            holidays.Add(easter.AddDays(57)); // Pingst (Pentecost)

            // Midsommardagen: The Saturday that falls between 20–26 June
            DateOnly midsummerStart = new (year, 6, 20);
            DateOnly midsummerEnd = new (year, 6, 26);
            AddSaturdayInRange(holidays, midsummerStart, midsummerEnd);

            // Alla helgons dag: The Saturday that falls between 31 Oct – 6 Nov
            DateOnly allHelgonsStart = new (year, 10, 31);
            DateOnly allHelgonsEnd = new (year, 11, 6);
            AddSaturdayInRange(holidays, allHelgonsStart, allHelgonsEnd);
        }

        return holidays.ToFrozenSet();
    }

    static void AddSaturdayInRange(HashSet<DateOnly> holidays, DateOnly start, DateOnly end)
    {
        int nrOfDays = end.DayNumber - start.DayNumber + 1;

        DateOnly holidaysInRange = Enumerable.Range(0, nrOfDays)
            .Select(start.AddDays)
            .First(d => d.DayOfWeek == DayOfWeek.Saturday);

        holidays.Add(holidaysInRange);
    }

    static DateOnly GetEasterSunday(int year)
    {
        // The Meeus/Jones/Butcher algorithm
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
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateOnly(year, month, day);
    }
}
