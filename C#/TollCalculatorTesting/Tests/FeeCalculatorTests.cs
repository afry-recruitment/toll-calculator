using TollCalculator.Controller;
using FluentAssertions;


namespace TollCalculatorTesting;

[TestClass]
public class FeeCalculatorTests
{
    [TestMethod]
    public void Low_Traffic_Time_Toll_Fee_Should_Be_8()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var times = new FeeCalculatorSeeds().GetLowTrafficPeriods();

        //ACT
        var result = new FeeCalculator().GetTollFeeByTime(times[0], vehicle);
        var result1 = new FeeCalculator().GetTollFeeByTime(times[1], vehicle);
        var result2 = new FeeCalculator().GetTollFeeByTime(times[2], vehicle);

        //ASSERT
        result.Should().Be(8);
        result1.Should().Be(8);
        result2.Should().Be(8);
    }

    [TestMethod]
    public void Medium_Traffic_Time_Toll_Fee_Should_Be_13()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var times = new FeeCalculatorSeeds().GetMediumTrafficPeriods();

        //ACT
        var result = new FeeCalculator().GetTollFeeByTime(times[0], vehicle);
        var result1 = new FeeCalculator().GetTollFeeByTime(times[1], vehicle);
        var result2 = new FeeCalculator().GetTollFeeByTime(times[2], vehicle);
        var result3 = new FeeCalculator().GetTollFeeByTime(times[3], vehicle);


        //ASSERT
        result.Should().Be(13);
        result1.Should().Be(13);
        result2.Should().Be(13);
        result3.Should().Be(13);

    }

    [TestMethod]
    public void High_Traffic_Time_Toll_Fee_Should_Be_18()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var times = new FeeCalculatorSeeds().GetHighrafficPeriods();

        //ACT
        var result = new FeeCalculator().GetTollFeeByTime(times[0], vehicle);
        var result1 = new FeeCalculator().GetTollFeeByTime(times[1], vehicle);

        //ASSERT
        result.Should().Be(18);
        result1.Should().Be(18);
    }

    [TestMethod]
    public void Night_Fee_Should_Be_Zero()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var time = new DateTime(2022, 09, 26, 18, 30, 20);
        var time1 = new DateTime(2022, 09, 26, 05, 29, 20);


        //ACT
        var result = new FeeCalculator().GetTollFeeByTime(time, vehicle);
        var result1 = new FeeCalculator().GetTollFeeByTime(time1, vehicle);


        //ASSERT
        result.Should().Be(0);
        result1.Should().Be(0);
    }

    [TestMethod]
    public void Toll_Fee_Should_Be_29()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var times = new FeeCalculatorSeeds().GetDublicatHoursPeriods();

        //ACT
        var result = new FeeCalculator().GetTollFee(vehicle, times);

        //ASSERT
        result.Should().Be(29);
    }

    [TestMethod]
    public void Toll_Fee_Should_Be_60_As_maximum_value()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var times = new FeeCalculatorSeeds().GetManyPeriods();

        //ACT
        var result = new FeeCalculator().GetTollFee(vehicle, times);

        //ASSERT
        result.Should().Be(60);
    }

    [TestMethod]
    public void Toll_Fee_Should_Be_Zero_At_Night()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetCarVehicle();
        var times = new List<DateTime>() { new DateTime(2022, 09, 26, 18, 30, 20), new DateTime(2022, 09, 26, 05, 29, 20) };

        //ACT
        var result = new FeeCalculator().GetTollFee(vehicle, times);

        //ASSERT
        result.Should().Be(0);
    }

    [TestMethod]
    public void Toll_Fee_Should_Be_Zero_For_MororBike()
    {
        //Arrang
        var vehicle = new VehicleSeeds().GetMotorbikeVehicle();
        var times = new List<DateTime>() { new DateTime(2022, 09, 26, 18, 30, 20), new DateTime(2022, 09, 26, 05, 29, 20) };

        //ACT
        var result = new FeeCalculator().GetTollFee(vehicle, times);

        //ASSERT
        result.Should().Be(0);
    }


}