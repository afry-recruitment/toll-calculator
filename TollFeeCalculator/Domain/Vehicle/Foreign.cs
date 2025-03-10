namespace TollFeeCalculator.Domain.Vehicle
{
    public class Foreign : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Foreign;
        }
    }
}