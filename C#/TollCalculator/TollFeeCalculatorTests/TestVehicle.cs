using TollFeeCalculatorLibrary;

namespace TollFeeCalculatorTests;

internal class TestVehicle(string VehicleType) : IVehicle
{
    readonly string vehicleType = VehicleType;

    public string GetVehicleType()
    {
        return vehicleType;
    }
}
