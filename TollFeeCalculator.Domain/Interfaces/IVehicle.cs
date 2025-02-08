using TollFeeCalculator.Domain.Enums;

namespace TollFeeCalculator.Domain.Interfaces
{
    public interface IVehicle
    {
        VehicleType VehicleType { get; }
        bool IsTollFree { get; }
    }
}
