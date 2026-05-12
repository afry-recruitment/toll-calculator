using Interfaces;
using Types;

namespace Models
{
    public class Foreign : IVehicle
    {
        public string GetVehicleType()
        {
            return TollFreeVehicles.Foreign.ToString();
        }
    }
}
