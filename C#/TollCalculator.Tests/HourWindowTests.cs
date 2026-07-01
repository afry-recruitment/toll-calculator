using TollCalculator.Models;

namespace TollCalculator.Tests;

public class HourWindowTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetTollFee_TwoPassesWithin60Min_ChargesHighest()
    {
        var dates = new[]
        {
            new DateTime(2026, 6, 23, 6, 0, 0),   // 8 SEK
            new DateTime(2026, 6, 23, 6, 30, 0)   // 13 SEK
        };
        Assert.Equal(13, _calculator.GetTollFee(new Car(), dates));
    }

    [Fact]
    public void GetTollFee_TwoPassesOver60Min_ChargesBoth()
    {
        var dates = new[]
        {
            new DateTime(2026, 6, 23, 6, 0, 0),   // 8 SEK
            new DateTime(2026, 6, 23, 7, 30, 0)   // 18 SEK
        };
        Assert.Equal(26, _calculator.GetTollFee(new Car(), dates));
    }

    [Fact]
    public void GetTollFee_ThreePassesTwoWindows_ChargesCorrectly()
    {
        var dates = new[]
        {
            new DateTime(2026, 6, 23, 6, 0, 0),   // 8 SEK  — window 1
            new DateTime(2026, 6, 23, 6, 45, 0),  // 13 SEK — window 1 (max)
            new DateTime(2026, 6, 23, 7, 30, 0)   // 18 SEK — window 2
        };
        Assert.Equal(31, _calculator.GetTollFee(new Car(), dates));
    }
}