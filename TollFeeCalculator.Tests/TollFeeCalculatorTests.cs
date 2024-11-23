using Xunit;
using FluentAssertions;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class TollFeeCalculatorTests
{
    [Theory]
    [InlineData(2024, 12, 2, 0, 0, 0, 0)]
    [InlineData(2024, 12, 2, 6, 0, 0, 8)]
    [InlineData(2024, 12, 2, 6, 30, 0, 13)]
    [InlineData(2024, 12, 2, 7, 0, 0, 18)]
    [InlineData(2024, 12, 2, 8, 0, 0, 13)]
    [InlineData(2024, 12, 2, 8, 30, 0, 8)]
    [InlineData(2024, 12, 2, 10, 0, 0, 0)]
    [InlineData(2024, 12, 2, 10, 30, 0, 8)]
    [InlineData(2024, 12, 2, 11, 0, 0, 0)]
    [InlineData(2024, 12, 2, 15, 0, 0, 13)]
    [InlineData(2024, 12, 2, 15, 31, 0, 18)]
    [InlineData(2024, 12, 2, 16, 0, 0, 18)]
    [InlineData(2024, 12, 2, 17, 0, 0, 13)]
    [InlineData(2024, 12, 2, 18, 0, 0, 8)]
    [InlineData(2024, 12, 2, 18, 30, 0, 0)]
    [InlineData(2024, 12, 1, 18, 0, 0, 0)]  // Saturday
    [InlineData(2024, 11, 30, 18, 0, 0, 0)] // Sunday
    [InlineData(2013, 1, 1, 0, 0, 0, 0)]    // New Year's Day
    public void GetTollFeeForCar_WhenSinglePass_ShouldReturnCorrectFee(
        int year,
        int month,
        int day,
        int hour,
        int minute,
        int second,
        int expectedFee)
    {
        var tollCalculator = new TollCalculator();

        var fee = tollCalculator.GetTollFee(new Car(), [new DateTime(year, month, day, hour, minute, second)]);
        fee.Should().Be(expectedFee);
    }

    [Fact]
    public void GetTollFeeForTollFreeVehicle_ShouldReturnZero()
    {
        var tollCalculator = new TollCalculator();

        var tollFreeVehicles = new List<Vehicle>
        {
            new Motorbike(),
            new Tractor(),
            new Emergency(),
            new Diplomat(),
            new Foreign(),
            new Military()
        };

        tollFreeVehicles.ForEach(Vehicle =>
        {
            var fee = tollCalculator.GetTollFee(Vehicle, [new DateTime(2024, 12, 2, 6, 30, 0)]);
            fee.Should().Be(0);
        });
    }

    [Fact]
    public void GetTollFee_ForOverlappingPassesWithinOneHour_ShouldReturnHighestRate()
    {
        var tollCalculator = new TollCalculator();

        var fee = tollCalculator.GetTollFee(new Car(), [
            new DateTime(2024, 12, 2, 6, 30, 0), 
            new DateTime(2024, 12, 2, 7, 30, 0)]);

        fee.Should().Be(18);
    }

    [Fact]
    public void GetTollFee_ForMultiplePasses_ShouldReturnTotalFee()
    {
        var tollCalculator = new TollCalculator();

        var fee = tollCalculator.GetTollFee(new Car(), [
            new DateTime(2024, 12, 2, 6, 30, 0),
            new DateTime(2024, 12, 2, 8, 30, 0)]);

        fee.Should().Be(21);
    }

    [Fact]
    public void GetTollFee_ForMultiplePasses_ShouldReturnMax60()
    {
        var tollCalculator = new TollCalculator();

        var fee = tollCalculator.GetTollFee(new Car(), [
            new DateTime(2024, 12, 2, 6, 45, 0),    
            new DateTime(2024, 12, 2, 7, 55, 0),    
            new DateTime(2024, 12, 2, 8, 45, 0),    
            new DateTime(2024, 12, 2, 10, 30, 0),   
            new DateTime(2024, 12, 2, 15, 00, 0),
            new DateTime(2024, 12, 2, 16, 15, 0)]);

        fee.Should().Be(60);
    }

    [Fact]
    public void GetTollFee_ForMultipleDays_ShouldReturnTotal()
    {
        var tollCalculator = new TollCalculator();

        var fee = tollCalculator.GetTollFee(new Car(), [
            new DateTime(2024, 12, 2, 6, 30, 0),
            new DateTime(2024, 12, 2, 8, 30, 0),
            new DateTime(2024, 12, 3, 6, 30, 0),
            new DateTime(2024, 12, 3, 8, 30, 0)]);

        fee.Should().Be(42);
    }
}