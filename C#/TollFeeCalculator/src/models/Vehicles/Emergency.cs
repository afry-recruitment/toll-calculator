using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Emergency : IVehicle
{
    public string GetVehicleType()
    {
        return "Emergency";
    }
}
