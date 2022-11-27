using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Models
{
    public class Emergency : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Emergency;
        }
    }
}