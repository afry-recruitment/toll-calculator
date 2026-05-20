using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    internal class Diplomat : IVehicle
    {
        public String GetVehicleType()
        {
            return "Diplomat";
        }
    }
}
