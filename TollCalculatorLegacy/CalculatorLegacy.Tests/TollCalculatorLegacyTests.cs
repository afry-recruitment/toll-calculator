using System;
using Calculator.Lib;
using Xunit;

namespace Calculator.Tests;

public class TollCalculatorLegacyTests
{
    private readonly TollCalculatorLegacy _calculator;
    private readonly Vehicle _car;
    private readonly Vehicle _motorbike;

    public TollCalculatorLegacyTests()
    {
        _calculator = new TollCalculatorLegacy();
        _car = new Car();
        _motorbike = new Motorbike();
    }

    [Fact]
    public void GetTollFee_TwoPassagesMoreThan60MinutesApart_ChargesBoth()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2013, 1, 2, 8, 5, 0)   // 13 SEK (65 minutes later)
        };

        var fee = _calculator.GetTollFee(_car, passages);

        // Should charge both: 18 + 13 = 31
        Assert.Equal(31, fee);
    }

    [Fact]
    public void GetTollFee_TwoPassagesWithin60Minutes_ChargesOnlyHighest()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2013, 1, 2, 7, 30, 0)  // 18 SEK (30 minutes later)
        };

        var fee = _calculator.GetTollFee(_car, passages);

        // Should charge only highest: 18
        Assert.Equal(18, fee);
    }

    [Fact]
    public void GetTollFee_TimeRange8_30to14_59_Returns8SEK()
    {
        var testTimes = new[]
        {
            new DateTime(2013, 1, 2, 8, 30, 0),  // 8:30
            new DateTime(2013, 1, 2, 9, 0, 0),   // 9:00
            new DateTime(2013, 1, 2, 10, 15, 0), // 10:15
            new DateTime(2013, 1, 2, 12, 45, 0), // 12:45
            new DateTime(2013, 1, 2, 14, 59, 0)  // 14:59
        };

        foreach (var time in testTimes)
        {
            var fee = _calculator.GetTollFee(time, _car);
            Assert.Equal(8, fee);
        }
    }

    [Fact]
    public void GetTollFee_TimeRange15_30to16_59_Returns18SEK()
    {
        var testTimes = new[]
        {
            new DateTime(2013, 1, 2, 15, 30, 0), // 15:30
            new DateTime(2013, 1, 2, 15, 45, 0), // 15:45
            new DateTime(2013, 1, 2, 16, 0, 0),  // 16:00
            new DateTime(2013, 1, 2, 16, 30, 0), // 16:30
            new DateTime(2013, 1, 2, 16, 59, 0)  // 16:59
        };

        foreach (var time in testTimes)
        {
            var fee = _calculator.GetTollFee(time, _car);
            Assert.Equal(18, fee);
        }
    }

    [Fact]
    public void GetTollFee_Time15_00to15_29_Returns13SEK()
    {
        var testTimes = new[]
        {
            new DateTime(2013, 1, 2, 15, 0, 0),  // 15:00
            new DateTime(2013, 1, 2, 15, 15, 0), // 15:15
            new DateTime(2013, 1, 2, 15, 29, 0)  // 15:29
        };

        foreach (var time in testTimes)
        {
            var fee = _calculator.GetTollFee(time, _car);
            Assert.Equal(13, fee);
        }
    }


    [Fact]
    public void GetTollFee_EmptyPassages_ReturnsZero()
    {
        var passages = new DateTime[] { };
        var fee = _calculator.GetTollFee(_car, passages);
        Assert.Equal(0, fee);
    }

    

    [Fact]
    public void GetTollFee_TollFreeVehicle_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 7, 0, 0)  // Would be 18 SEK for regular car
        };

        var fee = _calculator.GetTollFee(_motorbike, passages);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_Weekend_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 5, 7, 0, 0)  // Saturday, would be 18 SEK on weekday
        };

        var fee = _calculator.GetTollFee(_car, passages);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_Holiday_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 1, 7, 0, 0)  // New Year's Day 2013
        };

        var fee = _calculator.GetTollFee(_car, passages);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_DailyCapExceeded_Returns60()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 6, 0, 0),   // 8 SEK
            new DateTime(2013, 1, 2, 7, 30, 0),  // 18 SEK (90 min later)
            new DateTime(2013, 1, 2, 9, 0, 0),   // 8 SEK (90 min later)
            new DateTime(2013, 1, 2, 10, 30, 0), // 8 SEK (90 min later)
            new DateTime(2013, 1, 2, 15, 30, 0), // 18 SEK (new window)
            new DateTime(2013, 1, 2, 17, 0, 0)   // 13 SEK (90 min later)
        };
        // Total would be: 8 + 18 + 8 + 8 + 18 + 13 = 73 SEK

        var fee = _calculator.GetTollFee(_car, passages);
        
        // Should be capped at 60
        Assert.Equal(60, fee);
    }

    [Theory]
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(6, 59, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 30, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 0, 8)]
    [InlineData(12, 30, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    [InlineData(18, 30, 0)]
    [InlineData(22, 0, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(5, 59, 0)]
    public void GetTollFee_VariousTimes_ReturnsCorrectFee(int hour, int minute, int expectedFee)
    {
        var date = new DateTime(2013, 1, 2, hour, minute, 0);
        var fee = _calculator.GetTollFee(date, _car);
        Assert.Equal(expectedFee, fee);
    }


    [Fact]
    public void GetTollFee_ThreePassagesInSameWindow_ChargesHighest()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 6, 0, 0),  // 8 SEK
            new DateTime(2013, 1, 2, 6, 30, 0), // 13 SEK
            new DateTime(2013, 1, 2, 6, 50, 0)  // 13 SEK
        };

        var fee = _calculator.GetTollFee(_car, passages);
        
        // Should charge only highest: 13
        Assert.Equal(13, fee);
    }

    [Fact]
    public void GetTollFee_ComplexDayScenario_CalculatesCorrectly()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 6, 0, 0),   // Window 1: 8 SEK
            new DateTime(2013, 1, 2, 6, 30, 0),  // Same window: 13 SEK (higher)
            new DateTime(2013, 1, 2, 7, 30, 0),  // Window 2: 18 SEK (90 min after first)
            new DateTime(2013, 1, 2, 15, 0, 0),  // Window 3: 13 SEK
        };
        // Total: 13 + 18 + 13 = 44

        var fee = _calculator.GetTollFee(_car, passages);
        Assert.Equal(44, fee);
    }

    [Fact]
    public void GetTollFee_From_Multiple_Days_CalculateCorrectly()
    {
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 6, 0, 0),  // 8 SEK
            new DateTime(2013, 1, 3, 6, 30, 0), // 13 SEK
            new DateTime(2013, 1, 4, 6, 50, 0)  // 13 SEK
        };

        var fee = _calculator.GetTollFee(_car, passages);
        
        // Should charge 8 + 13 + 13
        Assert.Equal(34, fee);
    }
}