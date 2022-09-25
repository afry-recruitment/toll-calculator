using TollCalculator.Controller;
using FluentAssertions;

namespace TollCalculatorTesting;

[TestClass]
public class FreeVehicleTests
{
    [TestMethod]
    public void Car_Should_Not_Be_TollFree_Vahicle()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();

        //ACT
        var result = new FreeVehicle().IsTollFreeVehicle(vehicle);

        //ASSERT
        result.Should().Be(false);
    }

    [TestMethod]
    public void Motorbike_Should_Be_TollFree_Vahicle()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetMotorbikeVehicle();

        //ACT
        var result = new FreeVehicle().IsTollFreeVehicle(vehicle);

        //ASSERT
        result.Should().Be(true);
    }

    [TestMethod]
    public void Public_Holidays_Should_Be_TollFree_Dates()
    {
        //Arrang
        var newYearDate = new DateTime(2022, 1, 1);
        var christmasDate = new DateTime(2022, 12, 25);

        //ACT
        var result = new FreeVehicle().IsTollFreeDate(newYearDate);
        var result1 = new FreeVehicle().IsTollFreeDate(christmasDate);


        //ASSERT
        result.Should().Be(true);
        result1.Should().Be(true);
    }

    [TestMethod]
    public void Weekends_Should_Be_TollFree_Dates()
    {
        //Arrang
        var saturday = new DateTime(2022, 09, 24);
        var Sunday = new DateTime(2022, 09, 25);


        //ACT
        var result = new FreeVehicle().IsTollFreeDate(saturday);
        var result1 = new FreeVehicle().IsTollFreeDate(Sunday);


        //ASSERT
        result.Should().Be(true);
        result1.Should().Be(true);
    }

    [TestMethod]
    public void Normal_days_Should_NOT_Be_TollFree_Dates()
    {
        //Arrang
        var Thursday = new DateTime(2022, 09, 22);
        var Tuesday = new DateTime(2022, 09, 27);


        //ACT
        var result = new FreeVehicle().IsTollFreeDate(Thursday);
        var result1 = new FreeVehicle().IsTollFreeDate(Tuesday);


        //ASSERT
        result.Should().Be(false);
        result1.Should().Be(false);
    }
}