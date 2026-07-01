namespace TollCalculator.Tests;

public class FeeScheduleTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetFeeForTime_At0629_Returns8()
        => Assert.Equal(8, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 6, 29, 0)));

    [Fact]
    public void GetFeeForTime_At0630_Returns13()
        => Assert.Equal(13, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 6, 30, 0)));

    [Fact]
    public void GetFeeForTime_At0700_Returns18()
        => Assert.Equal(18, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 7, 0, 0)));

    [Fact]
    public void GetFeeForTime_At1529_Returns13()
        => Assert.Equal(13, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 15, 29, 0)));

    [Fact]
    public void GetFeeForTime_At1530_Returns18()
        => Assert.Equal(18, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 15, 30, 0)));

    [Fact]
    public void GetFeeForTime_At1829_Returns8()
        => Assert.Equal(8, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 18, 29, 0)));

    [Fact]
    public void GetFeeForTime_At1830_Returns0()
        => Assert.Equal(0, _calculator.GetFeeForTime(new DateTime(2026, 6, 23, 18, 30, 0)));
}