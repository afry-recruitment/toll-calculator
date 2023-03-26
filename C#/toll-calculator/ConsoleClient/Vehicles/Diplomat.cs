using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculatoric
{
    class Diplomat : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Diplomat.ToString();
        }
    }
}