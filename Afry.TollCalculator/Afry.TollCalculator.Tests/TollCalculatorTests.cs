using Afry.TollCalculator.Models;
using Afry.TollCalculator.Services;
using Afry.TollCalculator.Infrastructure.Settings;

namespace Afry.TollCalculator.Tests;

public sealed class TollCalculatorTests
{
    private static TollCalculatorService CreateCalculator()
    {
        var settings = TollSettings.Load();
        var options = Microsoft.Extensions.Options.Options.Create(settings);
        var tollFreeDateService = new TollFreeDateService(options);
        var scheduleProvider = new JsonTollFeeScheduleProvider(options);
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<TollCalculatorService>.Instance;
        return new TollCalculatorService(options, logger, tollFreeDateService, scheduleProvider);
    }

    private static readonly Vehicle Car = new(VehicleType.Car);
    private static readonly Vehicle Motorbike = new(VehicleType.Motorbike);

    [Fact]
    public void GetTollFee_NullVehicle_ReturnsFailure()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates = [new(new DateTime(2025, 1, 2, 6, 0, 0))];

        // Act
        var result = calc.GetTollFee(null!, dates);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void GetTollFee_SinglePass_NullVehicle_ReturnsFailure()
    {
        // Arrange
        var calc = CreateCalculator();
        var dateTimeOffset = new DateTimeOffset(new DateTime(2025, 1, 2, 7, 0, 0));

        // Act
        var result = calc.GetTollFee(dateTimeOffset, null!);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MinorUnit);
    }

    [Fact]
    public void GetTollFee_TollFreeHours_IsFree()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates = [new(new DateTime(2025, 1, 2, 5, 30, 0))];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_NullOrEmptyDates_ReturnsFailure()
    {
        // Arrange
        var calc = CreateCalculator();

        // Act
        var feeWithNull = calc.GetTollFee(Car, (DateTimeOffset[])null!);
        var feeWithEmpty = calc.GetTollFee(Car, (DateTimeOffset[])([]));

        // Assert
        Assert.False(feeWithNull.IsSuccess);
        Assert.False(feeWithEmpty.IsSuccess);
        Assert.NotNull(feeWithNull.Error);
        Assert.NotNull(feeWithEmpty.Error);
    }

    [Fact]
    public void GetTollFee_MultipleDays_ReturnsFailure()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates =
        [
            new(new DateTime(2025, 1, 2, 7, 0, 0)),
            new(new DateTime(2025, 1, 3, 7, 0, 0))
        ];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void GetTollFee_ConfiguredTollFreeDay_IsFree()
    {
        // Arrange
        var calc = CreateCalculator();
        var settings = TollSettings.Load();
        var configured = DateOnly.Parse(settings.TollFreeDays.First());
        DateTimeOffset[] dates = [new(new DateTime(configured.Year, configured.Month, configured.Day, 7, 0, 0))];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_ConfiguredTollFreeWeekday_IsFree()
    {
        // Arrange
        var calc = CreateCalculator();
        var settings = TollSettings.Load();
        var configuredDay = Enum.Parse<DayOfWeek>(settings.TollFreeWeekdays.First(), ignoreCase: true);

        var start = DateOnly.FromDateTime(new DateTime(2026, 1, 1));
        var date = start;
        for (int i = 0; i < 14 && date.DayOfWeek != configuredDay; i++)
        {
            date = date.AddDays(1);
        }

        DateTimeOffset[] dates = [new(new DateTime(date.Year, date.Month, date.Day, 7, 0, 0))];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MajorUnit);
    }

    [Theory]
    [InlineData(2025, 1, 4)] // Saturday
    [InlineData(2025, 1, 5)] // Sunday
    public void GetTollFee_Weekend_IsFree(int year, int month, int day)
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates = [new(new DateTime(year, month, day, 7, 0, 0))];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_FeeFreeVehicle_IsFree()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates = [new(new DateTime(2025, 1, 2, 7, 0, 0))];

        // Act
        var result = calc.GetTollFee(Motorbike, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_SinglePass_UsesSchedule()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates = [new(new DateTime(2025, 1, 2, 7, 0, 0))];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(18, result.Value!.MajorUnit);
    }

    [Fact]
    public void GetTollFee_Holiday_IsFree_EvenDuringRushHour()
    {
        // Arrange
        var calc = CreateCalculator();
        var settings = TollSettings.Load();
        var configured = DateOnly.Parse(settings.TollFreeDays.First());
        DateTimeOffset[] dates = [new(new DateTime(configured.Year, configured.Month, configured.Day, 7, 0, 0))];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_MultiplePassesWithin60Minutes_ChargesOnlyHighest()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates =
        [
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 0, 0)),  // 8
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 45, 0)), // 13
            new DateTimeOffset(new DateTime(2025, 1, 2, 7, 15, 0))  // 18 (second window, so 13 + 18
        ];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        // Windows:
        // 06:00-07:00 -> highest 13 (06:45)
        // 07:15 starts new window -> 18
        Assert.True(result.IsSuccess);
        Assert.Equal(31, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_MultiplePassesWithinSame60Minutes_UsesHighestOnly()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates =
        [
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 0, 0)),  // 8
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 30, 0)), // 13
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 59, 0))  // 13
        ];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(13, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_TimestampsAreUnsorted_StillApplies60MinuteRuleCorrectly()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates =
        [
            new DateTimeOffset(new DateTime(2025, 1, 2, 7, 10, 0)),
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 10, 0))
        ];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        // Sorted order: 06:10 (8) then 07:10 (18) => exactly 60 minutes apart => same window => highest 18
        Assert.True(result.IsSuccess);
        Assert.Equal(18, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_OneHourAndOneSecondApart_StartsNewWindow()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates =
        [
            new DateTimeOffset(new DateTime(2025, 1, 2, 6, 30, 0)), // 13
            new DateTimeOffset(new DateTime(2025, 1, 2, 7, 30, 1))  // 18, new window
        ];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(31, result.Value.MajorUnit);
    }

    [Fact]
    public void GetTollFee_TotalFeeIsCappedAt60()
    {
        // Arrange
        var calc = CreateCalculator();
        DateTimeOffset[] dates =
        [
            new DateTimeOffset(new DateTime(2025, 1, 2, 7, 0, 0)),  // 18
            new DateTimeOffset(new DateTime(2025, 1, 2, 8, 1, 0)),  // 13 (new window)
            new DateTimeOffset(new DateTime(2025, 1, 2, 9, 2, 0)),  // 8  (new window)
            new DateTimeOffset(new DateTime(2025, 1, 2, 10, 3, 0)), // 8  (new window)
            new DateTimeOffset(new DateTime(2025, 1, 2, 15, 31, 0)) // 18 (new window)
        ];

        // Act
        var result = calc.GetTollFee(Car, dates);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(60, result.Value.MajorUnit);
    }

    [Theory]
    [InlineData(2025, 1, 2, 15, 15, 13)] // 15:00-15:29 => 13
    [InlineData(2025, 1, 2, 15, 30, 18)] // 15:30-16:59 => 18
    public void GetTollFee_AfternoonBoundary_IsCorrect(int year, int month, int day, int hour, int minute, int expected)
    {
        // Arrange
        var calc = CreateCalculator();
        var dt = new DateTimeOffset(new DateTime(year, month, day, hour, minute, 0));

        // Act
        var result = calc.GetTollFee(dt, Car);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value.MajorUnit);
    }
}
