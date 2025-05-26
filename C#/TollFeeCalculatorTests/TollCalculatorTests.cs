using TollFeeCalculator.Models.Vehicles;
using TollFeeCalculator.Servcies;

namespace TollFeeCalculatorTests;

public class TollCalculatorTests
{
    [Fact]
    public void GetTollFee_VehicleIsNull_Shouldthrow()
    {
        Car car = new();
        Assert.Throws<ArgumentException>(() => TollCalculator.GetTollFee(null, []));
    }

    [Fact]
    public void GetTollFee_WhenDatesAreEmpty_ShouldReturnOk()
    {
        Car car = new();
        var fee = TollCalculator.GetTollFee(car, []);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_WhenVehicleIsTollFree_ShouldReturnZero()
    {
        Tractor tractor = new();
        var fee = TollCalculator.GetTollFee(tractor, [DateTime.Parse("2025-01-02T15:25:00")]);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_WhenDayIsWeekendOrHoliday_ShouldReturnZero()
    {
        Car car = new();
        var fee = TollCalculator.GetTollFee(car, [DateTime.Parse("2025-01-04T15:25:00")]); // Saturday
        Assert.Equal(0, fee);

        fee = TollCalculator.GetTollFee(car, [DateTime.Parse("2025-01-01T15:25:00")]); // New Year's Day
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_ChargedTwiceAnHour_ShouldReturnHigherValue()
    {
        Car car = new();
        var LowerFeeDateTime = DateTime.Parse("2025-01-02T15:25:00");
        var HigherFeeDateTime = DateTime.Parse("2025-01-02T15:35:00");
        var fee = TollCalculator.GetTollFee(car, [LowerFeeDateTime, HigherFeeDateTime]);
        Assert.Equal(18, fee);
    }
}