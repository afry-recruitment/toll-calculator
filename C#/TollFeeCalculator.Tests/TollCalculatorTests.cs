namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    [Fact]
    public void Car_in_morning_rush_hour_pays_18()
    {
        var calculator = new TollCalculator();
        var fee = calculator.GetTollFee(new DateTime(2013, 2, 4, 7, 0, 0), new Vehicle(VehicleType.Car));
        Assert.Equal(18, fee);
    }

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
    [InlineData(9, 0, 8)]
    [InlineData(9, 29, 8)]
    [InlineData(9, 30, 8)]
    [InlineData(12, 15, 8)]
    [InlineData(14, 0, 8)]
    [InlineData(14, 29, 8)]
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
    [InlineData(23, 59, 0)]
    [InlineData(0, 0, 0)]
    public void Fee_follows_the_schedule(int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        var passage = new DateTime(2013, 2, 4, hour, minute, 0);

        var fee = calculator.GetTollFee(passage, new Vehicle(VehicleType.Car));

        Assert.Equal(expectedFee, fee);
    }

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void Toll_free_vehicle_pays_nothing_in_rush_hour(VehicleType type)
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(new DateTime(2013, 2, 4, 7, 0, 0), new Vehicle(type));

        Assert.Equal(0, fee);
    }

    [Fact]
    public void Missing_vehicle_is_charged_as_a_paying_vehicle()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(new DateTime(2013, 2, 4, 7, 0, 0), null!);

        Assert.Equal(18, fee);
    }

    [Fact]
    public void Unknown_vehicle_type_is_charged_as_a_paying_vehicle()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(new DateTime(2013, 2, 4, 7, 0, 0), new Vehicle((VehicleType)99));

        Assert.Equal(18, fee);
    }
}