namespace TollFeeCalculator.Domain.Vehicle
{
    public class Tractor : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Tractor;
        }
    }
}