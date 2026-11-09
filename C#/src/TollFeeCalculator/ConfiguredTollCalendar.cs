namespace TollFeeCalculator;

/// <summary>
/// Immutable, explicit policy for one year. Holidays and any exempt holiday eves
/// must be supplied by the caller; weekends are always exempt.
/// </summary>
public sealed class ConfiguredTollCalendar : ITollCalendar
{
    private readonly int year;
    private readonly HashSet<DateOnly> exemptDates;
    private readonly bool exemptJuly;

    public ConfiguredTollCalendar(int year, IEnumerable<DateOnly> exemptDates, bool exemptJuly = true)
    {
        if (year is < 1 or > 9999) throw new ArgumentOutOfRangeException(nameof(year));
        ArgumentNullException.ThrowIfNull(exemptDates);

        this.year = year;
        this.exemptJuly = exemptJuly;
        this.exemptDates = new HashSet<DateOnly>(exemptDates);
        if (this.exemptDates.Any(date => date.Year != year))
            throw new ArgumentException("Every exempt date must belong to the configured year.", nameof(exemptDates));
    }

    public bool IsTollFree(DateOnly date)
    {
        if (date.Year != year)
            throw new ArgumentOutOfRangeException(nameof(date), date,
                $"The toll calendar only covers {year}. Configure an approved calendar for {date.Year}.");

        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
            || (exemptJuly && date.Month == 7)
            || exemptDates.Contains(date);
    }

    /// <summary>Preserves the exemption dates supplied by the exercise.</summary>
    public static ConfiguredTollCalendar For2013() => new(2013, new DateOnly[]
    {
        new(2013, 1, 1),
        new(2013, 3, 28), new(2013, 3, 29),
        new(2013, 4, 1), new(2013, 4, 30),
        new(2013, 5, 1), new(2013, 5, 8), new(2013, 5, 9),
        new(2013, 6, 5), new(2013, 6, 6), new(2013, 6, 21),
        new(2013, 11, 1),
        new(2013, 12, 24), new(2013, 12, 25), new(2013, 12, 26), new(2013, 12, 31)
    });
}
