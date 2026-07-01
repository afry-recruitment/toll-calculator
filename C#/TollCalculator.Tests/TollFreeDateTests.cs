using TollCalculator.Models;
using TollCalculator.Services;

namespace TollCalculator.Tests;

public class TollFreeDateTests
{
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetTollFee_OnSaturday_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { new DateTime(2026, 6, 27, 7, 0, 0) }));

    [Fact]
    public void GetTollFee_OnSunday_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { new DateTime(2026, 6, 28, 7, 0, 0) }));

    [Fact]
    public void GetTollFee_OnPublicHoliday_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { new DateTime(2026, 12, 25, 7, 0, 0) }));

    [Fact]
    public void GetTollFee_DayBeforePublicHoliday_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { new DateTime(2026, 12, 24, 7, 0, 0) }));

    [Fact]
    public void GetTollFee_InJuly_Returns0()
        => Assert.Equal(0, _calculator.GetTollFee(new Car(), new[] { new DateTime(2026, 7, 15, 7, 0, 0) }));

    [Fact]
    public void GetTollFee_OnWeekday_ReturnsFee()
        => Assert.True(_calculator.GetTollFee(new Car(), new[] { new DateTime(2026, 6, 23, 7, 0, 0) }) > 0);

    [Fact]
    public void IsTollFreeDate_DefaultDate_ThrowsArgumentException()
    {
        var holidayService = new HolidayService();
        Assert.Throws<ArgumentException>(() => holidayService.IsTollFreeDate(default));
    }
}