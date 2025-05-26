using TollFeeCalculator.Models.Contracts;

namespace TollFeeCalculator.Models.Vehicles;

public class Military : IVehicle
{
    public string GetVehicleType()
    {
        return "Military";
    }
}
