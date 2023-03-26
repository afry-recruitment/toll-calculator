using DataLib.Interfaces;
using DataLib.Enum;

namespace TollFeeCalculator
{
    public class Foreign : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Foreign.ToString();
        }
    }
}