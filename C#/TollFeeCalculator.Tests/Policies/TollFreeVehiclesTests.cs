using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests.Policies;

public class TollFreeVehiclesTests
{
    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void Exempt_vehicle_type_is_toll_free(VehicleType type)
    {
        Assert.True(TollFreeVehicles.Includes(new Vehicle(type)));
    }

    [Fact]
    public void Car_is_not_toll_free()
    {
        Assert.False(TollFreeVehicles.Includes(new Vehicle(VehicleType.Car)));
    }
}
