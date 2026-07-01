namespace TollCalculator.Models;

/// <summary>
/// Defines all supported vehicle types in the toll system.
/// </summary>
public enum VehicleType
{
    Car,
    Bus,
    Motorbike,
    Tractor,
    Emergency,
    Diplomat,
    Foreign,
    Military
}

/// <summary>
/// Extension methods for VehicleType.
/// Contains business rules for which vehicle types are exempt from toll fees.
/// To add or remove a toll-free vehicle type, update the TollFreeTypes set.
/// </summary>
public static class VehicleTypeExtensions
{
    private static readonly HashSet<VehicleType> TollFreeTypes = new()
    {
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    };

    /// <summary>
    /// Returns true if the vehicle type is exempt from toll fees.
    /// Toll-free types: Motorbike, Tractor, Emergency, Diplomat, Foreign, Military.
    /// </summary>
    public static bool IsTollFree(this VehicleType type)
    {
        if (!Enum.IsDefined(typeof(VehicleType), type))
            throw new ArgumentException($"Unknown vehicle type: {type}.", nameof(type));

        return TollFreeTypes.Contains(type);
    }
}