using Interfaces;
using Types;

namespace Models
{
    public class Car : IVehicle
    {
        public string GetVehicleType()
        {
            return TollPayingVehicleTypes.Car.ToString();
        }
    }
}