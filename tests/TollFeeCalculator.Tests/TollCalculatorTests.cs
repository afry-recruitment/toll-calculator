using System;
using System.Runtime.ConstrainedExecution;
using TollFeeCalculator;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

public class TollCalculatorTests
{
    [Theory]
    [InlineData(6, 0, 8)]      // 06:00 -> 8 kr
    [InlineData(6, 29, 8)]     // boundary: last minute of the 8 kr band
    [InlineData(6, 30, 13)]    // boundary: first minute of the 13 kr band
    [InlineData(6, 59, 13)]    // boundary: last minute of the 13 kr band
    [InlineData(7, 0, 18)]    // boundary: first minute of the 18 kr band
    [InlineData(7, 59, 18)]    // boundary: last minute of the 18 kr band
    [InlineData(8, 0, 13)]    // boundary: first minute of the 13 kr band
    [InlineData(8, 29, 13)]    // boundary: last minute of the 13 kr band
    [InlineData(8, 30, 8)]    // boundary: first minute of the 8 kr band
    [InlineData(14, 59, 8)]    // boundary: last minute of the 8 kr band
    [InlineData(15, 0, 13)]    // boundary: first minute of the 13 kr band
    [InlineData(15, 29, 13)]    // boundary: last minute of the 13 kr band
    [InlineData(15, 30, 18)]    // boundary: first minute of the 18 kr band
    [InlineData(16, 59, 18)]    // boundary: last minute of the 18 kr band
    [InlineData(17, 0, 13)]    // boundary: first minute of the 13 kr band
    [InlineData(17, 59, 13)]    // boundary: last minute of the 13 kr band
    [InlineData(18, 0, 8)]    // boundary: first minute of the 8 kr band
    [InlineData(18, 29, 8)]    // 	boundary: last minute of the 8 kr band
    [InlineData(18, 30, 0)]    // boundary: first minute of the 0 kr band
    [InlineData(5, 59, 0)]    // boundary: last minute of the 0 kr band

    public void GetTollFee_GivenTimeOfDay_ReturnsExpectedFee(int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 2, 7, hour, minute, 0); // a weekday (Thursday)

        int actualFee = calculator.GetTollFee(date, car);

        Assert.Equal(expectedFee, actualFee);
    }

    [Theory]
    [InlineData(7, 30, 0)]     // would normally cost 18 kr on a weekday, but the date is a toll-free Saturday
    [InlineData(15, 30, 0)]    // would normally cost 18 kr on a weekday, but the date is a toll-free Saturday
    [InlineData(5, 59, 0)]     // already fee-free on a weekday too; included as a control case

    public void GetTollFee_GivenTimeOfDay_On_A_Tollfree_Day_ReturnsExpectedFee(int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 2, 9, hour, minute, 0); // a tollfree day (saturday)

        int actualFee = calculator.GetTollFee(date, car);

        Assert.Equal(expectedFee, actualFee);
    }

    [Theory]
    [InlineData(2013, 7, 1, 7, 30, 0)]     // would normally cost 18 kr on a weekday, but the date is a toll-free day in July
    [InlineData(2013, 12, 23, 7, 30, 18)]    // costs 18 kr on a weekday, not toll-free
    [InlineData(2013, 12, 24, 7, 30, 0)]     // Toll free day (christmas eve)
    [InlineData(2014, 1, 1, 7, 30, 18)]     // Should be toll free day (new years day) but only year 2013 is covered in holiday logic.


    public void GetTollFee_On_A_Hardcoded_Holiday_Date_ReturnsExpectedFee(int year, int month, int day, int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        var car = new Car();

        var date = new DateTime(year, month, day, hour, minute, 0);
        int actualFee = calculator.GetTollFee(date, car);

        Assert.Equal(expectedFee, actualFee);
    }

    private class StubVehicle : Vehicle
    {
        private readonly string _vehicleType;
        
        public StubVehicle(string vehicleType)
        {
            _vehicleType = vehicleType;
        }

        public string GetVehicleType()
        {
            return _vehicleType;
        }
    }

    [Theory]
    [InlineData("Motorbike", 2013, 2, 7, 7, 30, 0)]    // would normally cost 18 kr but Motorbike is tollfree
    [InlineData("Tractor",  2013, 2, 7, 7, 30, 0)]     // would normally cost 18 kr but Tractor is tollfree
    [InlineData("Emergency", 2013, 2, 7, 7, 30, 0)]    // would normally cost 18 kr but Emergency is tollfree
    [InlineData("Diplomat", 2013, 2, 7, 7, 30, 0)]     // would normally cost 18 kr but Diplomat is tollfree
    [InlineData("Foreign", 2013, 2, 7, 7, 30, 0)]      // would normally cost 18 kr but Foreign is tollfree
    [InlineData("Military", 2013, 2, 7, 7, 30, 0)]     // would normally cost 18 kr but Military is tollfree
    [InlineData("Spaceship", 2013, 2, 7, 7, 30, 18)]   // Unrecognized vehicle type, treated as a regular (non-toll-free) vehicle.
    [InlineData("Car", 2013, 2, 7, 7, 30, 18)]         // Not tollfree, will cost 18
    
    public void GetTollFee_For_Tollfree_Vehicle_Types_ReturnsExpectedFee(string vehicle, int year, int month, int day, int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        Vehicle stubVehicle = new StubVehicle(vehicle);    
        var date = new DateTime(year, month, day, hour, minute, 0);
        int actualFee = calculator.GetTollFee(date, stubVehicle);

        Assert.Equal(expectedFee, actualFee);
    }

    [Fact]
    public void GetTollFee_For_Tollfree_Vehicle_Type_Motorbike_ReturnsExpectedFee()
    {
        var calculator = new TollCalculator();

        var motorbike = new Motorbike();
        var date = new DateTime(2013, 2, 7, 7, 30, 0);
        int actualFee = calculator.GetTollFee(date, motorbike);
        int expectedFee = 0; // Motorbike is tollfree

        Assert.Equal(expectedFee, actualFee);
    }
}