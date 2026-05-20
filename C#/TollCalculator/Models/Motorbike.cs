using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    public class Motorbike : IVehicle
    {
        public string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
