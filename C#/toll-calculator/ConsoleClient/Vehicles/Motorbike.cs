
using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculator
{
    public class Motorbike : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Motorbike.ToString();
        }
    }
}