using Interfaces;
using Types;

namespace Models
{
    public class Tractor : IVehicle
    {
        public string GetVehicleType()
        {
            return TollFreeVehicles.Tractor.ToString();
        }
    }
}
