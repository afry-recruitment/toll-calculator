using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculatoric
{
    public class Car : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Car.ToString();
        }
    }
}