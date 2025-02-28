using TollFeeCalculator.Enums;

namespace TollFeeCalculator.Models
{
    /// <inheritdoc />
    public class Vehicle(VehicleTypes vehicleType) : IVehicle
    {
        /// <inheritdoc />
        public VehicleTypes VehicleType => vehicleType;
    }
}