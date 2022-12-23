using FluentAssertions;
using TrafficToll.Internals.Services;
using TrafficToll.Internals.ValueObjects;
using Xunit;

namespace TrafficToll.Tests.ClassTests;


public class StaticTollTimeCalculatorTests
{
    [Fact]
    public void CorrectWithMaximumDailyFee_expect_adjusted()
    {
        StaticTollCalculator.CorrectWithMaximumDailyFee(100, 60).Should().Be(60);
    }

    [Fact]
    public void CorrectWithMaximumDailyFee_expect_not_adjusted()
    {
        StaticTollCalculator.CorrectWithMaximumDailyFee(100, 200).Should().Be(100);
    }

    [Fact]
    public void PassingsWhereThereIsNoHoliday_expect_remove_one_holiday_passing()
    {
        // Arrange
        var passings = new[]
        {
            new DateTime(2022,5,1,7,7,7),
            new DateTime(2022,5,2,7,7,7),
            new DateTime(2022,5,3,7,7,7),
        };

        var holidays = new[]
        {
            (2022,5,1)
        };

        // Act & Assert
        StaticTollCalculator.PassingsWhereThereIsNoHoliday(passings, holidays).Should().HaveCount(2);
    }

    [Fact]
    public void PassingsWhereThereIsNoHoliday_expect_unchanged()
    {
        // Arrange
        var passings = new[]
        {
            new DateTime(2022,5,1,7,7,7),
            new DateTime(2022,5,2,7,7,7),
            new DateTime(2022,5,3,7,7,7),
        };

        var holidays = Array.Empty<(int year, int month, int day)>();

        // Act & Assert
        StaticTollCalculator.PassingsWhereThereIsNoHoliday(passings, holidays).Should().HaveCount(3);
    }

    [Fact]
    public void PassingsWhichAreNotDuringWeekend_expect_adjusted_passings()
    {
        // Arrange

        var saturday = new DateTime(2022, 12, 24, 0, 0, 0);
        var sunday = new DateTime(2022, 12, 25, 0, 0, 0);
        var monday = new DateTime(2022, 12, 26, 0, 0, 0);

        var passings = new[]
        {
            saturday, sunday, monday
        };

        // Act & Assert
        StaticTollCalculator.PassingsWhichAreNotDuringWeekend(passings).Should().HaveCount(1);
    }

    [Fact]
    public void GetTollablePassings_expect_adjusted_passings()
    {
        // Arrange

        var saturday = new DateTime(2022, 12, 24, 0, 0, 0);
        var sunday = new DateTime(2022, 12, 25, 0, 0, 0);
        var monday = new DateTime(2022, 12, 26, 0, 0, 0);

        var passings = new[]
        {
            saturday, sunday, monday
        };

        var holidays = new[]
        {
            (2022,12,26)
        };

        // Act & Assert
        StaticTollCalculator.GetTollablePassings(passings, holidays).Should().HaveCount(0);
    }

    [Fact]
    public void TakePassingsWithin60MinutesTimeRange_expect_not_all()
    {
        // Arrange
        var passings = new[]
        {
            new DateTime(2022, 12, 24, 12, 0, 0),
            new DateTime(2022, 12, 24, 12, 59, 59, 999),
            new DateTime(2022, 12, 24, 13, 0, 0),
        };

        // Act & Assert
        StaticTollCalculator.TakePassingsWithin1HourTimeRange(passings, new TimeSpan(1,0,0)).Should().HaveCount(2);
    }


    [Fact]
    public void CalculateTotalSum_expect_correct_sum()
    {
        // Arrange
        var passings = new[]
        {
            new DateTime(2022, 12, 24, 12, 0, 0),
            new DateTime(2022, 12, 24, 12, 59, 59, 999),
            new DateTime(2022, 12, 24, 13, 0, 0),
        };

        var tollFeeSpans = new[]
        {
            new TollFeeSpan(
                start: new TimeSpan(12,0,0),
                end: new TimeSpan(13,0,0),
                tollPrice: 12),
            new TollFeeSpan(
                start: new TimeSpan(13,0,0),
                end: new TimeSpan(14,0,0),
                tollPrice: 13),
        };

        // Act & Assert
        StaticTollCalculator.CalculateTotalSum(passings, new TimeSpan(1, 0, 0), tollFeeSpans).Should().Be(25);
    }
}
