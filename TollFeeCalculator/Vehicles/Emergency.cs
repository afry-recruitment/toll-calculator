using TollFeeCalculator.Core;

namespace TollFeeCalculator.Vehicles
{
    public class Emergency : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Emergency;
        }
    }

}