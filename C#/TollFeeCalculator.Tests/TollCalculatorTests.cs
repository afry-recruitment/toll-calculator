using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    // Monday 4 February 2013 at 07:00: an ordinary weekday in rush hour.
    private static readonly DateTime RushHour = new(2013, 2, 4, 7, 0, 0);

    [Fact]
    public void Car_in_morning_rush_hour_pays_18()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, new Vehicle(VehicleType.Car));

        Assert.Equal(18, fee);
    }

    [Fact]
    public void Toll_free_vehicle_pays_nothing_in_rush_hour()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, new Vehicle(VehicleType.Motorbike));

        Assert.Equal(0, fee);
    }

    [Fact]
    public void Car_pays_nothing_on_a_toll_free_date()
    {
        var calculator = new TollCalculator();
        var saturdayRushHour = new DateTime(2013, 2, 2, 7, 0, 0);

        var fee = calculator.GetTollFee(saturdayRushHour, new Vehicle(VehicleType.Car));

        Assert.Equal(0, fee);
    }

    [Fact]
    public void Missing_vehicle_is_charged_as_a_paying_vehicle()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, null!);

        Assert.Equal(18, fee);
    }

    [Fact]
    public void Unknown_vehicle_type_is_charged_as_a_paying_vehicle()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, new Vehicle((VehicleType)99));

        Assert.Equal(18, fee);
    }
}