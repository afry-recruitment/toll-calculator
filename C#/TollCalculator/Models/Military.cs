using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    internal class Military : IVehicle
    {
        public String GetVehicleType()
        {
            return "Military";
        }
    }
}
