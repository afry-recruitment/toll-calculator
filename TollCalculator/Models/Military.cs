using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Models
{
    public class Military : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Military;
        }
    }
}