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

    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(31)]
    public void July_is_toll_free(int day)
    {
        Assert.True(TollFreeDates.Includes(new DateTime(2013, 7, day)));
    }
}