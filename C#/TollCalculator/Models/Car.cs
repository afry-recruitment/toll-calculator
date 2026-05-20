using TollCalculator.Interfaces;

namespace TollCalculator.Models
{
    public class Car : IVehicle
    {
        public String GetVehicleType()
        {
            return "Car";
        }
    }
}