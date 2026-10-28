using TollCalculator.Models;
using TollCalculator.Services;

namespace TollCalculator.Tests;

public class TollFeeCalculatorTests
{
    private readonly TollFeeService _calculator = new();

    // Individual fees

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
    [InlineData(19, 0, 0)]
    public void GetTollFee_ShouldReturnExpectedFee(
        int hour, int minute, int expectedFee)
    {
        var car = new Car();
        var date = new DateTime(2024, 1, 8, hour, minute, 0);

        int fee = _calculator.GetTollFee(date, car);

        Assert.Equal(expectedFee, fee);
    }

    // Input validation

    [Fact]
    public void GetTollFee_WithNullDates_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(
            () => _calculator.GetTollFee(new Car(), null!));
    }

    [Fact]
    public void GetTollFee_WithEmptyDates_ShouldReturnZero()
    {
        int fee = _calculator.GetTollFee(new Car(), Array.Empty<DateTime>());

        Assert.Equal(0, fee);
    }

    [Fact]
    public void PassagesFromDifferentDates_ShouldBeRejected()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 23, 50, 0),
            new DateTime(2024, 1, 9, 0, 10, 0)
        };

        Assert.Throws<ArgumentException>(
            () => _calculator.GetTollFee(new Car(), dates));
    }

    // One-hour charging rule

    [Fact]
    public void MultiplePassagesWithinOneHour_ShouldChargeHighestFee()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 6, 10, 0),
            new DateTime(2024, 1, 8, 6, 40, 0)
        };

        int fee = _calculator.GetTollFee(new Car(), dates);

        Assert.Equal(13, fee);
    }

    [Fact]
    public void PassageExactlyOneHourLater_ShouldRemainInSameInterval()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 6, 10, 0),
            new DateTime(2024, 1, 8, 7, 10, 0)
        };

        int fee = _calculator.GetTollFee(new Car(), dates);

        Assert.Equal(18, fee);
    }

    [Fact]
    public void PassageMoreThanOneHourLater_ShouldStartNewInterval()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 6, 10, 0),
            new DateTime(2024, 1, 8, 7, 11, 0)
        };

        int fee = _calculator.GetTollFee(new Car(), dates);

        Assert.Equal(26, fee);
    }

    [Fact]
    public void PassagesAcrossClockHour_ShouldRemainInSameInterval()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 6, 50, 0),
            new DateTime(2024, 1, 8, 7, 10, 0)
        };

        int fee = _calculator.GetTollFee(new Car(), dates);

        Assert.Equal(18, fee);
    }

    // Input ordering

    [Fact]
    public void UnorderedPassages_ShouldCalculateCorrectTotal()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 7, 11, 0),
            new DateTime(2024, 1, 8, 6, 40, 0),
            new DateTime(2024, 1, 8, 6, 10, 0)
        };

        int fee = _calculator.GetTollFee(new Car(), dates);

        Assert.Equal(31, fee);
    }

    [Fact]
    public void GetTollFee_ShouldNotModifyInputArray()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 7, 11, 0),
            new DateTime(2024, 1, 8, 6, 10, 0)
        };

        var originalDates = (DateTime[])dates.Clone();

        _calculator.GetTollFee(new Car(), dates);

        Assert.Equal(originalDates, dates);
    }

    // Daily maximum

    [Fact]
    public void DailyTotalBelowLimit_ShouldReturnActualTotal()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 7, 0, 0),
            new DateTime(2024, 1, 8, 9, 0, 0),
            new DateTime(2024, 1, 8, 11, 0, 0)
        };

        Assert.Equal(34, _calculator.GetTollFee(new Car(), dates));
    }

    [Fact]
    public void DailyTotalExactlyAtLimit_ShouldReturn60()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 7, 0, 0),
            new DateTime(2024, 1, 8, 9, 0, 0),
            new DateTime(2024, 1, 8, 11, 0, 0),
            new DateTime(2024, 1, 8, 13, 0, 0),
            new DateTime(2024, 1, 8, 15, 30, 0)
        };

        Assert.Equal(60, _calculator.GetTollFee(new Car(), dates));
    }

    [Fact]
    public void DailyTotalAboveLimit_ShouldBeCappedAt60()
    {
        var dates = new[]
        {
            new DateTime(2024, 1, 8, 7, 0, 0),
            new DateTime(2024, 1, 8, 9, 0, 0),
            new DateTime(2024, 1, 8, 11, 0, 0),
            new DateTime(2024, 1, 8, 13, 0, 0),
            new DateTime(2024, 1, 8, 15, 30, 0),
            new DateTime(2024, 1, 8, 17, 30, 0)
        };

        Assert.Equal(60, _calculator.GetTollFee(new Car(), dates));
    }

    // Vehicle exemptions

    [Fact]
    public void Motorbike_ShouldBeTollFree()
    {
        var date = new DateTime(2024, 1, 8, 7, 0, 0);

        Assert.Equal(0, _calculator.GetTollFee(date, new Motorbike()));
    }

    [Fact]
    public void CarDuringRushHour_ShouldBeCharged()
    {
        var date = new DateTime(2024, 1, 8, 7, 0, 0);

        Assert.Equal(18, _calculator.GetTollFee(date, new Car()));
    }

    // Weekends and holidays

    [Theory]
    [InlineData(2024, 1, 6)]  // Saturday
    [InlineData(2024, 1, 7)]  // Sunday
    [InlineData(2013, 1, 1)]  // Listed holiday
    [InlineData(2013, 7, 15)] // July holiday period
    public void TollFreeDate_ShouldReturnZero(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 7, 0, 0);

        Assert.Equal(0, _calculator.GetTollFee(date, new Car()));
    }

    [Fact]
    public void OrdinaryWeekday_ShouldNotBeTollFree()
    {
        var date = new DateTime(2024, 1, 8, 7, 0, 0);

        Assert.Equal(18, _calculator.GetTollFee(date, new Car()));
    }
}
