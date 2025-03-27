using TollFeeCalculator.Core;

namespace TollFeeCalculator.Vehicles
{
    public class Diplomat : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Diplomat;
        }
    }

}