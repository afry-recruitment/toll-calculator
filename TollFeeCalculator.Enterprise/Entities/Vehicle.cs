using TollFeeCalculator.Enterprise.Enums;

namespace TollFeeCalculator.Enterprise.Entities
{
    public sealed class Vehicle
    {
        public VehicleType Type { get; init; }

        public Vehicle(VehicleType type)
        {
            Type = type;
        }
    }
}