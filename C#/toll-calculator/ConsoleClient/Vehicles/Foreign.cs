using ConsoleClient.Interfaces;
using ConsoleClient.Enum;

namespace TollFeeCalculator
{
    public class Foreign : IVehicle
    {
        public string? RegNumber { get; }

        public string GetVehicleType()
        {
            return Vehicles.Foreign.ToString();
        }
    }
}