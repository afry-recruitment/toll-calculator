using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculator
{
    public class Tractor : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Tractor.ToString();
        }
    }
}