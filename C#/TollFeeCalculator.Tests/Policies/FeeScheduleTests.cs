using TollFeeCalculator.Policies;

namespace TollFeeCalculator.Tests.Policies;

public class FeeScheduleTests
{
    [Theory]
    [InlineData(5, 59, 0)]
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(6, 59, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 0, 8)]
    [InlineData(9, 29, 8)]
    [InlineData(9, 30, 8)]
    [InlineData(12, 15, 8)]
    [InlineData(14, 0, 8)]
    [InlineData(14, 29, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]
    [InlineData(23, 59, 0)]
    [InlineData(0, 0, 0)]
    public void Fee_follows_the_schedule(int hour, int minute, int expectedFee)
    {
        var fee = FeeSchedule.FeeAt(new TimeOnly(hour, minute));

        Assert.Equal(expectedFee, fee);
    }
}