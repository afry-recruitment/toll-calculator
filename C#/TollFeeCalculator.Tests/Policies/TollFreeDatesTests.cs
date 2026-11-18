using TollFeeCalculator.Policies;

namespace TollFeeCalculator.Tests.Policies;

public class TollFreeDatesTests
{
    [Theory]
    [InlineData(2, 2)] // Saturday
    [InlineData(2, 3)] // Sunday
    public void Weekend_is_toll_free(int month, int day)
    {
        Assert.True(TollFreeDates.Includes(new DateTime(2013, month, day)));
    }

    [Theory]
    [InlineData(2, 4)] // Monday
    [InlineData(6, 28)] // Friday, the last weekday before July
    [InlineData(8, 1)] // Thursday, the first weekday after July
    public void Ordinary_weekday_is_not_toll_free(int month, int day)
    {
        Assert.False(TollFreeDates.Includes(new DateTime(2013, month, day)));
    }

    [Theory]
    [InlineData(1, 1)] // New Year's Day
    [InlineData(3, 28)] // Maundy Thursday
    [InlineData(3, 29)] // Good Friday
    [InlineData(4, 1)] // Easter Monday
    [InlineData(4, 30)] // Walpurgis Night
    [InlineData(5, 1)] // May Day
    [InlineData(5, 8)] // Day before Ascension Day
    [InlineData(5, 9)] // Ascension Day
    [InlineData(6, 5)] // Day before National Day
    [InlineData(6, 6)] // National Day
    [InlineData(6, 21)] // Midsummer Eve
    [InlineData(11, 1)] // Day before All Saints' Day
    [InlineData(12, 24)] // Christmas Eve
    [InlineData(12, 25)] // Christmas Day
    [InlineData(12, 26)] // Boxing Day
    [InlineData(12, 31)] // New Year's Eve
    public void Holiday_or_day_before_holiday_in_2013_is_toll_free(int month, int day)
    {
        Assert.True(TollFreeDates.Includes(new DateTime(2013, month, day)));
    }

    // Only the holiday and the day before it are toll-free, not the days around them.
    [Theory]
    [InlineData(1, 2)] // Wednesday, the day after New Year's Day
    [InlineData(3, 27)] // Wednesday, two days before Good Friday
    [InlineData(4, 2)] // Tuesday, the day after Easter Monday
    [InlineData(5, 10)] // Friday, the day after Ascension Day
    [InlineData(6, 4)] // Tuesday, two days before National Day
    [InlineData(6, 7)] // Friday, the day after National Day
    [InlineData(12, 23)] // Monday, two days before Christmas Day
    [InlineData(12, 27)] // Friday, the day after Boxing Day
    public void Weekday_next_to_a_holiday_in_2013_is_not_toll_free(int month, int day)
    {
        Assert.False(TollFreeDates.Includes(new DateTime(2013, month, day)));
    }

    // The dates Transportstyrelsen lists as exempt in Gothenburg for 2026,
    // in addition to weekends and July.
    [Theory]
    [InlineData(1, 1)] // New Year's Day
    [InlineData(1, 5)] // Day before Epiphany
    [InlineData(1, 6)] // Epiphany
    [InlineData(4, 2)] // Maundy Thursday
    [InlineData(4, 3)] // Good Friday
    [InlineData(4, 6)] // Easter Monday
    [InlineData(4, 30)] // Walpurgis Night
    [InlineData(5, 1)] // May Day
    [InlineData(5, 13)] // Day before Ascension Day
    [InlineData(5, 14)] // Ascension Day
    [InlineData(6, 5)] // Day before National Day
    [InlineData(6, 19)] // Midsummer Eve
    [InlineData(10, 30)] // Day before All Saints' Day
    [InlineData(12, 24)] // Christmas Eve
    [InlineData(12, 25)] // Christmas Day
    [InlineData(12, 31)] // New Year's Eve
    public void Holiday_or_day_before_holiday_in_2026_is_toll_free(int month, int day)
    {
        Assert.True(TollFreeDates.Includes(new DateTime(2026, month, day)));
    }

    [Theory]
    [InlineData(2, 2)] // Monday
    [InlineData(6, 30)] // Tuesday, the last weekday before July
    [InlineData(8, 3)] // Monday, the first weekday after July
    public void Ordinary_weekday_in_2026_is_not_toll_free(int month, int day)
    {
        Assert.False(TollFreeDates.Includes(new DateTime(2026, month, day)));
    }

    // Leap years, with Easter on 12 April 2020 and 31 March 2024.
    [Theory]
    [InlineData(2020, 4, 9)] // Maundy Thursday
    [InlineData(2020, 4, 10)] // Good Friday
    [InlineData(2020, 4, 13)] // Easter Monday
    [InlineData(2020, 5, 20)] // Day before Ascension Day
    [InlineData(2020, 5, 21)] // Ascension Day
    [InlineData(2020, 6, 19)] // Midsummer Eve
    [InlineData(2020, 10, 30)] // Day before All Saints' Day
    [InlineData(2024, 3, 28)] // Maundy Thursday
    [InlineData(2024, 3, 29)] // Good Friday
    [InlineData(2024, 4, 1)] // Easter Monday
    [InlineData(2024, 5, 8)] // Day before Ascension Day
    [InlineData(2024, 5, 9)] // Ascension Day
    [InlineData(2024, 6, 21)] // Midsummer Eve
    [InlineData(2024, 11, 1)] // Day before All Saints' Day
    public void Holiday_or_day_before_holiday_in_a_leap_year_is_toll_free(int year, int month, int day)
    {
        Assert.True(TollFreeDates.Includes(new DateTime(year, month, day)));
    }

    [Fact]
    public void Leap_day_is_an_ordinary_weekday()
    {
        // 29 February 2024 was a Thursday.
        Assert.False(TollFreeDates.Includes(new DateTime(2024, 2, 29)));
    }

    [Theory]
    [InlineData(2013, 1)]
    [InlineData(2013, 15)]
    [InlineData(2013, 31)]
    [InlineData(2026, 1)]
    [InlineData(2026, 15)]
    [InlineData(2026, 31)]
    public void July_is_toll_free(int year, int day)
    {
        Assert.True(TollFreeDates.Includes(new DateTime(year, 7, day)));
    }
}