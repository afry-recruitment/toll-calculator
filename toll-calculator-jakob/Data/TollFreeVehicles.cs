using toll_calculator_jakob.Enums;

namespace toll_calculator_jakob.Data
{
    public static class TollFreeVehicles
    {
        public static readonly IEnumerable<VehicleType> TollFreeVehicleTypes =
        [
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military
        ];
    }
}
