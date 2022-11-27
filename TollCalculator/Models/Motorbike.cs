using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Models
{
    public class Motorbike : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Motorbike;
        }
    }
}
