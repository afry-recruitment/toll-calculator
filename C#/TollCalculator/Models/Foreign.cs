using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    internal class Foreign : IVehicle
    {
        public String GetVehicleType()
        {
            return "Foreign";
        }
    }
}
