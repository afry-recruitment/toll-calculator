using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculator
{
    public class Emergency : IVehicle
    {
        public string? RegNumber { get; }

        public string GetVehicleType()
        {
            return Vehicles.Emergency.ToString();
        }
    }
}