using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Motorbike : IVehicle
{
    public string GetVehicleType()
    {
        return "Motorbike";
    }
}
