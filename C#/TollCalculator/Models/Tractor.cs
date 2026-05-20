using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    internal class Tractor : IVehicle
    {
        public String GetVehicleType()
        {
            return "Tractor";
        }
    }
}
