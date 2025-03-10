namespace TollFeeCalculator.Domain.Vehicle
{
    public class Emergency : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Emergency;
        }
    }
}