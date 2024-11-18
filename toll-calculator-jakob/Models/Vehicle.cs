using toll_calculator_jakob.Data;
using toll_calculator_jakob.Enums;

namespace toll_calculator_jakob.Models
{
    public class Vehicle(VehicleType type) : IVehicle
    {
        public VehicleType Type => type;
        public bool IsTollFree => TollFreeVehicles.TollFreeVehicleTypes.Contains(Type);
    }
}