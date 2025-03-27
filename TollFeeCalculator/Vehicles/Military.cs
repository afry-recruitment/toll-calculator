using TollFeeCalculator.Core;

namespace TollFeeCalculator.Vehicles
{
    public class Military : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Military;
        }
    }

}