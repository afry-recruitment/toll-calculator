using DataLib.Enum;
using DataLib.Interfaces;

namespace TollFeeCalculatoric
{
    class Car : IVehicle
    {
        public string GetVehicleType()
        {
            return Vehicles.Car.ToString();
        }
    }
}