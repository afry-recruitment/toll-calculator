using TollCalculator.Services.Policies;

namespace TollCalculator.Tests.Policies;

public class DefaultTollFeeScheduleTests
{
    private readonly DefaultTollFeeSchedule _sut = new();

    [Theory]
    [InlineData(2013, 1, 2, 6, 0, 8)]
    [InlineData(2013, 1, 2, 6, 29, 8)]
    [InlineData(2013, 1, 2, 6, 30, 13)]
    [InlineData(2013, 1, 2, 7, 0, 18)]
    [InlineData(2013, 1, 2, 8, 15, 13)]
    [InlineData(2013, 1, 2, 8, 30, 8)]
    [InlineData(2013, 1, 2, 14, 59, 8)]
    [InlineData(2013, 1, 2, 15, 0, 13)]
    [InlineData(2013, 1, 2, 15, 30, 18)]
    [InlineData(2013, 1, 2, 16, 59, 18)]
    [InlineData(2013, 1, 2, 17, 0, 13)]
    [InlineData(2013, 1, 2, 18, 29, 8)]
    [InlineData(2013, 1, 2, 18, 30, 0)]
    [InlineData(2013, 1, 2, 5, 59, 0)]
    public void GetFee_ReturnsExpectedFee(int y, int m, int d, int h, int min, int expected)
    {
        var date = new DateTime(y, m, d, h, min, 0);
        var fee = _sut.GetFee(date);
        Assert.Equal(expected, fee);
    }
}