using TollCalculator;
using TollCalculator.Models;
using TollCalculator.Settings;

namespace TollCalculator.Tests;

public class ExceptionTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetTollFee_NullVehicle_ThrowsArgumentNullException()
    {
        var dates = new[] { new DateTime(2026, 6, 23, 7, 0, 0) };
        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(null!, dates));
    }

    [Fact]
    public void GetTollFee_NullDates_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(new Car(), null!));
    }

    [Fact]
    public void GetTollFee_EmptyDates_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _calculator.GetTollFee(new Car(), Array.Empty<DateTime>()));
    }

    [Fact]
    public void TollSettings_NegativeMaxDailyFee_ThrowsArgumentException()
    {
        var settings = new TollSettings();
        Assert.Throws<ArgumentException>(() => settings.MaxDailyFee = -1);
    }

    [Fact]
    public void TollSettings_NegativeHourWindowMinutes_ThrowsArgumentException()
    {
        var settings = new TollSettings();
        Assert.Throws<ArgumentException>(() => settings.HourWindowMinutes = -1);
    }

    [Fact]
    public void TollSettings_NullFeeSchedule_ThrowsArgumentNullException()
    {
        var settings = new TollSettings();
        Assert.Throws<ArgumentNullException>(() => settings.FeeSchedule = null!);
    }
}