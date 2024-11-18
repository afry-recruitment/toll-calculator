using toll_calculator_jakob.Enums;

namespace toll_calculator_jakob.Models
{
    public interface IVehicle
    {
        VehicleType Type { get; }
        bool IsTollFree { get; }
    }
}