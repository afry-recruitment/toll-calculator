using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Foreign : IVehicle
{
    public string GetVehicleType()
    {
        return "Foreign";
    }
}
