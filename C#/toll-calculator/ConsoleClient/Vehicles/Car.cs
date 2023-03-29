using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculatoric
{
    public class Car : IVehicle
    {
        public string RegNumber { get; }

        public string GetVehicleType()
        {
            return Vehicles.Car.ToString();
        }
        public Car(string regNumber) 
        {
           RegNumber = regNumber;
        }
    }
}