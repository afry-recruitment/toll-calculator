using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculator
{
    public class Military : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Military.ToString();
        }
    }
}