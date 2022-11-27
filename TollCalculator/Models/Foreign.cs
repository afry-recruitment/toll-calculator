using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Models
{
    public class Foreign : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Foreign;
        }
    }
}