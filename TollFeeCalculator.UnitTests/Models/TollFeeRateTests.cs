using TollFeeCalculator.Models;

namespace TollFeeCalculator.UnitTests.Models;

public class TollFeeRateTests
{
    private readonly TollFeeRate _sut = new(new TimeOnly(6, 0), new TimeOnly(6, 30), 8);

    [Fact]
    public void IsWithinTimeInterval_TimeIsOutsideInterval_ReturnsFalse()
    {
        var result = _sut.IsWithinTimeInterval(new TimeOnly(5, 0));

        Assert.False(result);
    }

    [Fact]
    public void IsWithinTimeInterval_TimeIsWithinInterval_ReturnsTrue()
    {
        var result = _sut.IsWithinTimeInterval(new TimeOnly(6, 10));

        Assert.True(result);
    }
}