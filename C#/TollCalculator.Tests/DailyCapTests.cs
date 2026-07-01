using TollCalculator.Models;

namespace TollCalculator.Tests;

public class DailyCapTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetTollFee_ExceedsDailyCap_Returns60()
    {
        var dates = new[]
        {
            new DateTime(2026, 6, 23, 7, 0, 0),
            new DateTime(2026, 6, 23, 8, 30, 0),
            new DateTime(2026, 6, 23, 10, 0, 0),
            new DateTime(2026, 6, 23, 11, 30, 0),
            new DateTime(2026, 6, 23, 13, 0, 0),
            new DateTime(2026, 6, 23, 15, 30, 0),
            new DateTime(2026, 6, 23, 17, 0, 0),
        };
        Assert.Equal(60, _calculator.GetTollFee(new Car(), dates));
    }

    [Fact]
    public void GetTollFee_BelowDailyCap_ReturnsExactFee()
    {
        var dates = new[] { new DateTime(2026, 6, 23, 7, 0, 0) };
        Assert.Equal(18, _calculator.GetTollFee(new Car(), dates));
    }
}