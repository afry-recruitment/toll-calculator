using Interfaces;
using Types;

namespace Models
{
    public class Diplomat : IVehicle
    {
        public string GetVehicleType()
        {
            return TollFreeVehicles.Diplomat.ToString();
        }
    }
}
