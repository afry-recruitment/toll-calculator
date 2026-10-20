namespace TollFeeCalculator.Tests;

public class TollCalculatorExemptionTests
{
    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void ExemptVehicle_OnChargeableDay_PaysNothingInBothMethods(VehicleType vehicle)
    {
        var calculator = new TollCalculator();
        var passages = new[]
        {
            new DateTime(2013, 1, 2, 7, 0, 0),
            new DateTime(2013, 1, 2, 9, 0, 0),
            new DateTime(2013, 1, 2, 16, 0, 0)
        };

        foreach (var passage in passages)
        {
            Assert.Equal(0, calculator.GetTollFee(passage, vehicle));
        }
        Assert.Equal(0, calculator.GetTollFee(vehicle, passages));
    }

    [Theory]
    [InlineData(1, 5)] // Saturday.
    [InlineData(1, 6)] // Sunday.
    // The 2013 exemption list documented in limitations.md.
    [InlineData(1, 1)]
    [InlineData(3, 28)]
    [InlineData(3, 29)]
    [InlineData(4, 1)]
    [InlineData(4, 30)]
    [InlineData(5, 1)]
    [InlineData(5, 8)]
    [InlineData(5, 9)]
    [InlineData(6, 5)]
    [InlineData(6, 6)]
    [InlineData(6, 21)]
    [InlineData(11, 1)]
    [InlineData(12, 24)]
    [InlineData(12, 25)]
    [InlineData(12, 26)]
    [InlineData(12, 31)]
    public void ExemptDate_CarPaysNothingInBothMethods(int month, int day)
    {
        AssertCarPaysNothingOn(month, day);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(31)]
    public void JulyBeginningMiddleAndEnd_CarPaysNothingInBothMethods(int day)
    {
        AssertCarPaysNothingOn(7, day);
    }

    [Fact]
    public void FirstWeekdayAfterJuly_CarPaysExpectedFee()
    {
        var calculator = new TollCalculator();
        var car = VehicleType.Car;
        var passage = new DateTime(2013, 8, 1, 7, 0, 0);

        Assert.Equal(18, calculator.GetTollFee(passage, car));
        Assert.Equal(18, calculator.GetTollFee(car, new[] { passage }));
    }

    private static void AssertCarPaysNothingOn(int month, int day)
    {
        var calculator = new TollCalculator();
        var car = VehicleType.Car;
        var passages = new[]
        {
            new DateTime(2013, month, day, 7, 0, 0),
            new DateTime(2013, month, day, 9, 0, 0),
            new DateTime(2013, month, day, 16, 0, 0)
        };

        foreach (var passage in passages)
        {
            Assert.Equal(0, calculator.GetTollFee(passage, car));
        }
        Assert.Equal(0, calculator.GetTollFee(car, passages));
    }
}
