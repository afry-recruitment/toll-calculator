
namespace TollCalculatorSystem.Models
{
    public class Motorbike : IVehicle
    {
        public VehicleType GetVehicleType()
        {
            return VehicleType.Motorbike;
        }
    }
}
