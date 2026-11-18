using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Policies;

public static class TollFreeVehicles
{
    private static readonly HashSet<VehicleType> TollFreeTypes =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military,
    ];

    /// <summary>
    /// Tells whether a vehicle is exempt from toll fees.
    /// A missing vehicle, or one with an unknown type, is not exempt.
    /// </summary>
    public static bool Includes(IVehicle? vehicle)
    {
        if (vehicle == null) return false;
        return TollFreeTypes.Contains(vehicle.Type);
    }
}