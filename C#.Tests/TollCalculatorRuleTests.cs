using TollFeeCalculator;

namespace TollFeeCalculator.Tests;

public class TollCalculatorRuleTests
{
    [Theory]
    [InlineData(6, 0, 0, 8)]
    [InlineData(6, 30, 8, 13)]
    [InlineData(7, 0, 13, 18)]
    [InlineData(8, 0, 18, 13)]
    [InlineData(8, 30, 13, 8)]
    [InlineData(15, 0, 8, 13)]
    [InlineData(15, 30, 13, 18)]
    [InlineData(17, 0, 18, 13)]
    [InlineData(18, 0, 13, 8)]
    [InlineData(18, 30, 8, 0)]
    public void EveryTariffBoundary_ChangesAtExactTime(int hour, int minute, int before, int atBoundary)
    {
        var calculator = new TollCalculator();
        var car = new Vehicle(VehicleType.Car);
        var boundary = new DateTime(2013, 1, 2, hour, minute, 0);

        Assert.Equal(before, calculator.GetTollFee(boundary.AddTicks(-1), car));
        Assert.Equal(atBoundary, calculator.GetTollFee(boundary, car));
    }

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void ExemptVehicle_MultipleRushHourWindows_ReturnsZero(VehicleType type)
    {
        var calculator = new TollCalculator();
        var vehicle = new Vehicle(type);
        var morning = new DateTime(2013, 1, 2, 7, 0, 0);
        var afternoon = new DateTime(2013, 1, 2, 16, 0, 0);

        Assert.Equal(0, calculator.GetTollFee(vehicle, new[] { morning, afternoon }));
        Assert.Equal(0, calculator.GetTollFee(morning, vehicle));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(7)]
    [InlineData(999)]
    public void UnknownVehicleType_IsRejected(int type)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Vehicle((VehicleType)type));
    }

    [Theory]
    [InlineData(DateTimeKind.Unspecified)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    public void PassageTimes_AreUsedAsSuppliedRegardlessOfKind(DateTimeKind kind)
    {
        var calculator = new TollCalculator();
        var car = new Vehicle(VehicleType.Car);
        var first = new DateTime(2013, 1, 2, 6, 0, 0, kind);
        var second = new DateTime(2013, 1, 2, 6, 30, 0, kind);
        var third = new DateTime(2013, 1, 2, 7, 15, 0, kind);

        Assert.Equal(8, calculator.GetTollFee(first, car));
        Assert.Equal(31, calculator.GetTollFee(car, new[] { first, second, third }));
    }

    [Theory]
    [InlineData(2012)]
    [InlineData(2014)]
    [InlineData(2026)]
    public void DefaultCalculator_UnsupportedYear_IsRejectedByBothMethods(int year)
    {
        var calculator = new TollCalculator();
        var car = new Vehicle(VehicleType.Car);
        var date = new DateTime(year, 1, 1, 7, 0, 0);

        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetTollFee(date, car));
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetTollFee(car, new[] { date }));
    }

    [Fact]
    public void UnsupportedYear_IsRejectedEvenForExemptVehicle()
    {
        var calculator = new TollCalculator();
        var vehicle = new Vehicle(VehicleType.Motorbike);
        var date = new DateTime(2026, 1, 1, 7, 0, 0);

        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetTollFee(date, vehicle));
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetTollFee(vehicle, new[] { date }));
    }

    [Fact]
    public void SuppliedCalendar_IsUsedByBothMethods()
    {
        var exemptDay = new DateOnly(2026, 9, 29);
        var calculator = new TollCalculator(new TollCalendar(2026, new[] { exemptDay }));
        var car = new Vehicle(VehicleType.Car);
        var passage = exemptDay.ToDateTime(new TimeOnly(7, 0));

        Assert.Equal(0, calculator.GetTollFee(passage, car));
        Assert.Equal(0, calculator.GetTollFee(car, new[] { passage }));
        Assert.Equal(18, calculator.GetTollFee(passage.AddDays(1), car));
        Assert.Equal(18, calculator.GetTollFee(car, new[] { passage.AddDays(1) }));
    }

    [Fact]
    public void Constructor_NullCalendar_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TollCalculator(null!));
    }

    [Fact]
    public void TotalExactlyAtDailyLimit_ReturnsSixty()
    {
        var calculator = new TollCalculator();
        var dates = new[]
        {
            new DateTime(2013, 1, 2, 6, 0, 0),
            new DateTime(2013, 1, 2, 7, 1, 0),
            new DateTime(2013, 1, 2, 8, 2, 0),
            new DateTime(2013, 1, 2, 9, 3, 0),
            new DateTime(2013, 1, 2, 15, 0, 0)
        };

        Assert.Equal(60, calculator.GetTollFee(new Vehicle(VehicleType.Car), dates));
    }

    [Fact]
    public void ReusingCalculator_DoesNotCarryFeesBetweenDays()
    {
        var calculator = new TollCalculator();
        var car = new Vehicle(VehicleType.Car);
        var firstDay = new[] { new DateTime(2013, 1, 2, 7, 0, 0) };
        var secondDay = new[] { new DateTime(2013, 1, 3, 6, 0, 0) };

        Assert.Equal(18, calculator.GetTollFee(car, firstDay));
        Assert.Equal(8, calculator.GetTollFee(car, secondDay));
        Assert.Equal(18, calculator.GetTollFee(car, firstDay));
    }
}
