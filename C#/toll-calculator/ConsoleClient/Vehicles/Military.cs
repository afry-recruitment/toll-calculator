using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

namespace TollFeeCalculator
{
    public class Military : IVehicle
    {
        public string RegNumber { get; }

        public string GetVehicleType()
        {
            return Vehicles.Military.ToString();
        }
        public Military(string regNumber) { RegNumber = regNumber; }

    }
}