namespace TollFeeCalculator.Domain.Vehicle
{
    public class Military : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Military;
        }
    }
}