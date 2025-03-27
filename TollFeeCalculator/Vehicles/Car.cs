using TollFeeCalculator.Core;

namespace TollFeeCalculator.Vehicles
{
    public class Car : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Car;
        }
    }


}