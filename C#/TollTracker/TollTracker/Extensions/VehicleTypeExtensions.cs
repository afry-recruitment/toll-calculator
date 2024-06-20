using System.Collections.ObjectModel;
using TollTracker.Enums;

namespace TollTracker.Extensions;

public static class VehicleTypeExtensions
{
    private static readonly ReadOnlyCollection<VehicleType> TollFreeVehicleTypes =
    new ReadOnlyCollection<VehicleType>(
        new List<VehicleType> {
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military,
        });

    public static bool IsExemptFromToll(this VehicleType vehicleType)
    {
        return TollFreeVehicleTypes.Contains(vehicleType);
    }
}

