using TollFeeCalculator;

namespace TollFeeCalculator.Tests;

public class TollCalendarTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 28)]
    [InlineData(3, 29)]
    [InlineData(4, 1)]
    [InlineData(4, 30)]
    [InlineData(5, 1)]
    [InlineData(5, 8)]
    [InlineData(5, 9)]
    [InlineData(6, 5)]
    [InlineData(6, 6)]
    [InlineData(6, 21)]
    [InlineData(11, 1)]
    [InlineData(12, 24)]
    [InlineData(12, 25)]
    [InlineData(12, 26)]
    [InlineData(12, 31)]
    public void OriginalCalendar_ExplicitExemption_IsTollFree(int month, int day)
    {
        Assert.True(TollCalendar.Original2013.IsTollFree(new DateOnly(2013, month, day)));
    }

    [Fact]
    public void OriginalCalendar_AllOfJuly_IsTollFree()
    {
        for (int day = 1; day <= 31; day++)
        {
            Assert.True(TollCalendar.Original2013.IsTollFree(new DateOnly(2013, 7, day)));
        }
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 27)]
    [InlineData(4, 2)]
    [InlineData(6, 7)]
    [InlineData(8, 1)]
    [InlineData(12, 23)]
    [InlineData(12, 27)]
    public void OriginalCalendar_OrdinaryWeekday_IsChargeable(int month, int day)
    {
        Assert.False(TollCalendar.Original2013.IsTollFree(new DateOnly(2013, month, day)));
    }

    [Theory]
    [InlineData(26)]
    [InlineData(27)]
    public void CustomCalendar_Weekend_IsTollFreeWithoutExplicitExemption(int day)
    {
        var calendar = new TollCalendar(2026, Array.Empty<DateOnly>());

        Assert.True(calendar.IsTollFree(new DateOnly(2026, 9, day)));
    }

    [Fact]
    public void CustomCalendar_CopiesSuppliedDates()
    {
        var holiday = new DateOnly(2026, 9, 29);
        var dates = new List<DateOnly> { holiday };
        var calendar = new TollCalendar(2026, dates);
        dates.Clear();

        Assert.True(calendar.IsTollFree(holiday));
        Assert.False(calendar.IsTollFree(holiday.AddDays(1)));
    }

    [Fact]
    public void Calendar_UnsupportedYear_ThrowsEvenForWeekend()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => TollCalendar.Original2013.IsTollFree(new DateOnly(2014, 1, 4)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10000)]
    public void Constructor_InvalidYear_Throws(int year)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TollCalendar(year, Array.Empty<DateOnly>()));
    }

    [Fact]
    public void Constructor_NullExemptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TollCalendar(2013, null!));
    }

    [Fact]
    public void Constructor_ExemptionFromAnotherYear_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => new TollCalendar(2013, new[] { new DateOnly(2014, 1, 1) }));
    }
}
