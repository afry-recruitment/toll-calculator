using AFRY.TollCalculator.API.Domain.Models;
using AFRY.TollCalculator.API.Features.CalculateTollfee;
using AFRY.TollCalculator.API.UnitTests.Mocks;
using Moq;

namespace AFRY.TollCalculator.API.UnitTests;

public class CalculateTollFeeServiceTests
{
    private readonly CalculateTollFeeService _calculateTollFeeService;
    private readonly Mock<ICalculateTollFeeRepository> _mockTollFeeRepository;

    public CalculateTollFeeServiceTests()
    {
        _mockTollFeeRepository = RepositoryMocks.GetTollFeeRepository();
        _calculateTollFeeService = new CalculateTollFeeService(_mockTollFeeRepository.Object);
    }

    [Fact]
    public void IsTollFreeVehicle_WithNullVehicle_ReturnsFalse()
    {
        Vehicle vehicle = null;

        bool result = _calculateTollFeeService.IsTollFreeVehicle(vehicle);

        Assert.False(result);
    }

    [Fact]
    public void IsTollFreeVehicle_WithMotorbike_ReturnsTrue()
    {
        Vehicle vehicle = new Motorbike();

        bool result = _calculateTollFeeService.IsTollFreeVehicle(vehicle);

        Assert.True(result);
    }

    [Fact]
    public void IsTollFreeVehicle_WithCar_ReturnsFalse()
    {
        Vehicle vehicle = new Car();

        bool result = _calculateTollFeeService.IsTollFreeVehicle(vehicle);

        Assert.False(result);
    }

    [Fact]
    public void IsTollFreeDate_WithWeekend_ReturnsTrue()
    {
        var saturdayDate = new DateTime(2013, 7, 6); // A Saturday
        var sundayDate = new DateTime(2013, 7, 7); // A Sunday

        var resultSaturday = _calculateTollFeeService.IsTollFreeDate(saturdayDate);
        var resultSunday = _calculateTollFeeService.IsTollFreeDate(sundayDate);

        Assert.True(resultSaturday);
        Assert.True(resultSunday);
    }

    [Theory]
    [InlineData("2013-01-01", true)] // New Year's Day
    [InlineData("2013-12-25", true)] // Christmas Day
    [InlineData("2013-02-01", false)] // Random non-toll-free date
    [InlineData("2013-07-15", false)] // Another random non-toll-free date
    public void IsTollFreeDate_ValidatesDifferentDates(string dateString, bool expectedResult)
    {
        var testDate = DateTime.Parse(dateString);

        var result = _calculateTollFeeService.IsTollFreeDate(testDate);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("2013-01-01", "Motorbike", 0)] // Toll-free date (New Year's Day)
    [InlineData("2013-02-01", "Motorbike", 0)] // Toll-free vehicle (Motorbike)
    [InlineData("2013-02-01", "Car", 8, 6, 0)] // Regular toll fee for a time between 06:00 - 06:29
    [InlineData("2013-02-01", "Car", 13, 6, 30)] // Higher toll fee for a time between 06:30 - 06:59
    [InlineData("2013-02-01", "Car", 0, 22, 0)] // No toll fee outside of toll period (late evening)
    public void GetTollFee_WithVariousInputs_ReturnsExpectedFee(string dateString, string vehicleType, int expectedFee, int hour = 0, int minute = 0)
    {
        var date = DateTime.Parse(dateString).AddHours(hour).AddMinutes(minute);
        Vehicle vehicle = vehicleType == "Motorbike" ? new Motorbike() : new Car();

        var result = _calculateTollFeeService.GetTollFee(date, vehicle);

        Assert.Equal(expectedFee, result);
    }

    [Fact]
    public void GetTollFee_WithMultipleDates_CalculatesTotalFeeCorrectly()
    {
        Vehicle vehicle = new Car();

        DateTime[] dates = {
            new DateTime(2013, 2, 1, 6, 15, 0),  // 6:15 AM (8 fee)
            new DateTime(2013, 2, 1, 6, 45, 0),  // 6:45 AM (13 fee)
            new DateTime(2013, 2, 1, 7, 15, 0),  // 7:15 AM (18 fee)
            new DateTime(2013, 2, 1, 8, 10, 0),  // 8:10 AM (13 fee)
            new DateTime(2013, 2, 1, 15, 10, 0)  // 15:10 PM (13 fee)
        };

        var result = _calculateTollFeeService.GetTollFee(vehicle, dates);

        Assert.Equal(44, result); 
    }

    [Fact]
    public void GetTollFee_WithinSameInterval_CalculatesCorrectFee()
    {
        Vehicle vehicle = new Car();

        DateTime[] dates = {
            new DateTime(2013, 2, 1, 6, 15, 0),  // 6:15 AM (8 fee)
            new DateTime(2013, 2, 1, 6, 30, 0)   // 6:30 AM (13 fee)
        };

        var result = _calculateTollFeeService.GetTollFee(vehicle, dates);

        Assert.Equal(13, result); 
    }

    [Fact]
    public void GetTollFee_WhenMaxLimitReached_ReturnsSixty()
    {
        Vehicle vehicle = new Car();

        DateTime[] dates = {
            new DateTime(2013, 2, 1, 6, 0, 0),  // 6:00 AM (8 fee)
            new DateTime(2013, 2, 1, 6, 30, 0),  // 6:30 AM (13 fee)
            new DateTime(2013, 2, 1, 7, 0, 0),  // 7:00 AM (18 fee)
            new DateTime(2013, 2, 1, 8, 0, 0),  // 8:00 AM (13 fee)
            new DateTime(2013, 2, 1, 9, 0, 0),  // 9:00 AM (8 fee)
            new DateTime(2013, 2, 1, 15, 0, 0)  // 15:00 PM (13 fee)
        };

        var result = _calculateTollFeeService.GetTollFee(vehicle, dates);

        Assert.Equal(60, result);
    }


}