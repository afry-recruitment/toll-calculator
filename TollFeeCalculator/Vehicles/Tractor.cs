using TollFeeCalculator.Core;

namespace TollFeeCalculator.Vehicles
{
    public class Tractor : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Tractor;
        }
        public class Emergency : IVehicle
        {
            public VehicleType GetVehicleType()
            {
                return VehicleType.Emergency;
            }
        }
    }

}