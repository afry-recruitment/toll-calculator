using Interfaces;
using Types;

namespace Models
{
    public class Motorbike : IVehicle
    {
        public string GetVehicleType()
        {
            return TollFreeVehicles.Motorbike.ToString();
        }
    }
}
