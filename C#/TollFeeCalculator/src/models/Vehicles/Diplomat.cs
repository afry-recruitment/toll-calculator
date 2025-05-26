using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Diplomat : IVehicle
{
    public string GetVehicleType()
    {
        return "Diplomat";
    }
}
