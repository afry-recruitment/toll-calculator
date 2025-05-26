using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Car : IVehicle
{
    public string GetVehicleType()
    {
        return "Car";
    }
}
