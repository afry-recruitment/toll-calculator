namespace TollFeeCalculator.Tests;

public class SwedishHolidaysTests
{
    [Theory]
    [InlineData(2013, "2013-03-31")]
    [InlineData(2024, "2024-03-31")]
    [InlineData(2025, "2025-04-20")]
    [InlineData(2026, "2026-04-05")]
    [InlineData(2038, "2038-04-25")]
    public void EasterSunday_IsCalculatedCorrectly(int year, string easter) =>
        Assert.Contains(DateOnly.Parse(easter), SwedishHolidays.GetPublicHolidays(year));

    [Fact]
    public void Holidays2026_MatchOfficialCalendar()
    {
        DateOnly[] expected =
        [
            new(2026, 1, 1), new(2026, 1, 6), new(2026, 4, 3), new(2026, 4, 5), new(2026, 4, 6),
            new(2026, 5, 1), new(2026, 5, 14), new(2026, 5, 24), new(2026, 6, 6), new(2026, 6, 20),
            new(2026, 10, 31), new(2026, 12, 25), new(2026, 12, 26),
        ];
        Assert.Equal(expected.Order(), SwedishHolidays.GetPublicHolidays(2026).Order());
    }

    [Fact]
    public void TollFreeDates2013_MatchOriginalHardcodedList()
    {
        // Weekdays outside July that the original implementation listed as toll-free for 2013.
        DateOnly[] original =
        [
            new(2013, 1, 1), new(2013, 3, 28), new(2013, 3, 29), new(2013, 4, 1), new(2013, 4, 30),
            new(2013, 5, 1), new(2013, 5, 8), new(2013, 5, 9), new(2013, 6, 5), new(2013, 6, 6),
            new(2013, 6, 21), new(2013, 11, 1), new(2013, 12, 24), new(2013, 12, 25), new(2013, 12, 26),
            new(2013, 12, 31),
        ];

        var calculated = Enumerable.Range(0, 365)
            .Select(i => new DateOnly(2013, 1, 1).AddDays(i))
            .Where(d => d.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday) && d.Month != 7)
            .Where(TollCalculator.IsTollFreeDate);

        Assert.Equal(original, calculated);
    }
}
