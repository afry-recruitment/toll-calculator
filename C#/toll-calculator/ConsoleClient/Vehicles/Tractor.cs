using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculator
{
    public class Tractor : IVehicle
    {
        public string? RegNumber { get; }

        public string GetVehicleType()
        {
            return Vehicles.Tractor.ToString();
        }
    }
}