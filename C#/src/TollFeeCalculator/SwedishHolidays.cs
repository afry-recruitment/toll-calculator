using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace TollFeeCalculator;

/// <summary>Swedish public holidays ("röda dagar"), calculated for any year.</summary>
public static class SwedishHolidays
{
    private static readonly ConcurrentDictionary<int, FrozenSet<DateOnly>> Cache = new();

    public static bool IsPublicHoliday(DateOnly date) => GetPublicHolidays(date.Year).Contains(date);

    public static IReadOnlySet<DateOnly> GetPublicHolidays(int year) => Cache.GetOrAdd(year, CalculatePublicHolidays);

    private static FrozenSet<DateOnly> CalculatePublicHolidays(int year)
    {
        var easter = GetEasterSunday(year);
        return new[]
        {
            new DateOnly(year, 1, 1),              // Nyårsdagen
            new DateOnly(year, 1, 6),              // Trettondedag jul
            easter.AddDays(-2),                    // Långfredagen
            easter,                                // Påskdagen
            easter.AddDays(1),                     // Annandag påsk
            new DateOnly(year, 5, 1),              // Första maj
            easter.AddDays(39),                    // Kristi himmelsfärdsdag
            easter.AddDays(49),                    // Pingstdagen
            new DateOnly(year, 6, 6),              // Sveriges nationaldag
            FirstSaturdayFrom(year, 6, 20),        // Midsommardagen
            FirstSaturdayFrom(year, 10, 31),       // Alla helgons dag
            new DateOnly(year, 12, 25),            // Juldagen
            new DateOnly(year, 12, 26),            // Annandag jul
        }.ToFrozenSet();
    }

    private static DateOnly FirstSaturdayFrom(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);
        return date.AddDays(((int)DayOfWeek.Saturday - (int)date.DayOfWeek + 7) % 7);
    }

    // Anonymous Gregorian algorithm (Meeus/Jones/Butcher).
    private static DateOnly GetEasterSunday(int year)
    {
        int a = year % 19, b = year / 100, c = year % 100, d = b / 4, e = b % 4;
        int f = (b + 8) / 25, g = (b - f + 1) / 3, h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4, k = c % 4, l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31, day = (h + l - 7 * m + 114) % 31 + 1;
        return new DateOnly(year, month, day);
    }
}
