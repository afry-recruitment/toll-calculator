using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculator
{
    public class Emergency : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Emergency.ToString();
        }
    }
}