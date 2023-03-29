using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculatoric
{
    class Diplomat : IVehicle
    {
        public string? RegNumber { get; }

        public string GetVehicleType()
        {
            return Vehicles.Diplomat.ToString();
        }
    }
}