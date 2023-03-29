using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculatoric
{
    public class Diplomat : IVehicle
    {
        public string RegNumber { get; }
        public string GetVehicleType() { return Vehicles.Diplomat.ToString(); }
        public Diplomat(string regNumber) { RegNumber = regNumber; }
    }
}