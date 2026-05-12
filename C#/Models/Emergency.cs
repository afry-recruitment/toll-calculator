using Interfaces;
using Types;

namespace Models
{
    public class Emergency : IVehicle
    {
        public string GetVehicleType()
        {
            return TollFreeVehicles.Emergency.ToString();
        }
    }
}
