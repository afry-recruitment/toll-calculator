using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Models
{
    public class Diplomat : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Diplomat;
        }
    }
}