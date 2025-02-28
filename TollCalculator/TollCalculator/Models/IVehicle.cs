
using TollFeeCalculator.Enums;

namespace TollFeeCalculator.Models
{
    /// <summary>
    /// Vehicle definition 
    /// </summary>
    public interface IVehicle
    {
        /// <inheritdoc />
        public VehicleTypes VehicleType { get; }
    }
}