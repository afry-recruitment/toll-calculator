using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Tractor : IVehicle
{
    public string GetVehicleType()
    {
        return "Tractor";
    }
}
