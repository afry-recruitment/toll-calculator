namespace TollFeeCalculator.Tests;

public class TollFeeScheduleTests
{
    [Theory]
    [InlineData("00:00", 0)]
    [InlineData("05:59", 0)]
    [InlineData("06:00", 8)]
    [InlineData("06:29", 8)]
    [InlineData("06:30", 13)]
    [InlineData("06:59", 13)]
    [InlineData("07:00", 18)]
    [InlineData("07:59", 18)]
    [InlineData("08:00", 13)]
    [InlineData("08:29", 13)]
    [InlineData("08:30", 8)]
    [InlineData("09:00", 8)] // original code returned 0 for xx:00-xx:29 between 09 and 14
    [InlineData("12:15", 8)]
    [InlineData("14:59", 8)]
    [InlineData("15:00", 13)]
    [InlineData("15:29", 13)]
    [InlineData("15:30", 18)]
    [InlineData("16:59", 18)]
    [InlineData("17:00", 13)]
    [InlineData("17:59", 13)]
    [InlineData("18:00", 8)]
    [InlineData("18:29", 8)]
    [InlineData("18:30", 0)]
    [InlineData("23:59", 0)]
    public void GetFee_ReturnsFeeForTimeOfDay(string time, int expected) =>
        Assert.Equal(expected, TollFeeSchedule.GetFee(TimeOnly.Parse(time)));
}
