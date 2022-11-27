using TollFeeCalculator.Models;

namespace TollFeeCalculator.Interfaces
{
    public interface IVehicle
    {
        VehicleType GetVehicleType();
    }
}