namespace TollFeeCalculator;

/// <summary>Weekend exemptions and explicitly supplied toll-free dates for one year.</summary>
public sealed class TollCalendar
{
    private readonly HashSet<DateOnly> _tollFreeDates;

    public TollCalendar(int year, IEnumerable<DateOnly> tollFreeDates)
    {
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 1 and 9999.");
        }

        ArgumentNullException.ThrowIfNull(tollFreeDates);
        _tollFreeDates = new HashSet<DateOnly>(tollFreeDates);
        if (_tollFreeDates.Any(date => date.Year != year))
        {
            throw new ArgumentException("All exemption dates must belong to the calendar year.", nameof(tollFreeDates));
        }

        Year = year;
    }

    public int Year { get; }

    /// <summary>The exemption dates supplied by the original assignment, including July.</summary>
    public static TollCalendar Original2013 { get; } = CreateOriginal2013();

    public bool IsTollFree(DateOnly date)
    {
        if (date.Year != Year)
        {
            throw new ArgumentOutOfRangeException(nameof(date), date, $"This calendar only supports {Year}.");
        }

        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
            || _tollFreeDates.Contains(date);
    }

    private static TollCalendar CreateOriginal2013()
    {
        DateOnly[] exemptions =
        [
            new(2013, 1, 1),
            new(2013, 3, 28), new(2013, 3, 29),
            new(2013, 4, 1), new(2013, 4, 30),
            new(2013, 5, 1), new(2013, 5, 8), new(2013, 5, 9),
            new(2013, 6, 5), new(2013, 6, 6), new(2013, 6, 21),
            new(2013, 11, 1),
            new(2013, 12, 24), new(2013, 12, 25), new(2013, 12, 26), new(2013, 12, 31)
        ];

        var july = Enumerable.Range(1, 31).Select(day => new DateOnly(2013, 7, day));
        return new TollCalendar(2013, exemptions.Concat(july));
    }
}
