namespace TollFeeCalculator.Domain.Vehicle
{
    public class Car : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Car;
        }
    }
}