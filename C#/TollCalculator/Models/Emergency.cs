using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    internal class Emergency : IVehicle
    {
        public String GetVehicleType()
        {
            return "Emergency";
        }
    }
}
