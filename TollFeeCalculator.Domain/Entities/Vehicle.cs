using TollFeeCalculator.Domain.Enums;
using TollFeeCalculator.Domain.Interfaces;

namespace TollFeeCalculator.Domain.Entities
{
    public class Vehicle(VehicleType vehicleType) : IVehicle
    {
        public VehicleType VehicleType => vehicleType;

        public bool IsTollFree => TollFreeVehicles.Contains(vehicleType);

        private static readonly List<VehicleType> TollFreeVehicles = [
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military
        ];
    }
}
