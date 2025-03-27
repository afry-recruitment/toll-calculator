using TollFeeCalculator.Core;

namespace TollFeeCalculator.Vehicles
{
    public class Foreign : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Foreign;
        }
    }

}