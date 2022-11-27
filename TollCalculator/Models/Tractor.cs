using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Models
{
    public class Tractor : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Tractor;
        }
    }
}