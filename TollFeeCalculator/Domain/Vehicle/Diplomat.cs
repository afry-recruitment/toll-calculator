namespace TollFeeCalculator.Domain.Vehicle
{
    public class Diplomat : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Diplomat;
        }
    }
}