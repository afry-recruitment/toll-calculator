
using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculator
{
    public class Motorbike : IVehicle
    {
        public string RegNumber { get; }
        public string GetVehicleType() { return Vehicles.Motorbike.ToString(); }
        public Motorbike(string regNumber) { RegNumber = regNumber; }
    }
}