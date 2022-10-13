using TollFeeCalculator.Utilities;

namespace TollFeeCalculator.Models
{
    public interface IVehicle
    {
        string GetVehicleType();
        bool SetTollFreeStatus();
        void ChangeTollFreeStatus(bool tollFreeStatus);
        void ChangeVehicleType(VehicleType type);
    }
}
