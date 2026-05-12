using Interfaces;
using Types;

namespace Models
{
    public class Military : IVehicle
    {
        public string GetVehicleType()
        {
            return TollFreeVehicles.Military.ToString();
        }
    }
}
