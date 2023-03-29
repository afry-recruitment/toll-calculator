using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculator
{
    public class Tractor : IVehicle
    {
        public string RegNumber { get; }
        public string GetVehicleType() { return Vehicles.Tractor.ToString(); }
        public Tractor(string regNumber) { RegNumber = regNumber; }
    }
}