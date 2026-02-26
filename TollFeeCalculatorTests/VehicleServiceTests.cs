using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;
using TollFeeCalculator.Services;

namespace TollFeeCalculatorTests;

public class VehicleServiceTests
{
    private readonly IVehicleService _service;

    public VehicleServiceTests()
    {
        var config = new VehicleConfiguration
        {
            TollFreeVehicleTypes = new[] { "Motorcycle", "Bus", "Emergency" }
        };
        _service = new VehicleService(config);
    }

    [Theory]
    [InlineData("Motorcycle", true)]
    [InlineData("Bus", true)]
    [InlineData("Emergency", true)]
    [InlineData("Car", false)]
    [InlineData("Truck", false)]
    public void IsTollFree_ReturnsCorrectResult(string vehicleType, bool expected)
    {
        var result = _service.IsTollFree(vehicleType);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsTollFree_CaseInsensitive_ReturnsTrue()
    {
        Assert.True(_service.IsTollFree("motorcycle"));
        Assert.True(_service.IsTollFree("MOTORCYCLE"));
        Assert.True(_service.IsTollFree("MoToRcYcLe"));
    }
}