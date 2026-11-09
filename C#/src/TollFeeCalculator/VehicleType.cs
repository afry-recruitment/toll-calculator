namespace TollFeeCalculator;

/// <summary>Vehicle categories recognized by the exercise policy.</summary>
public enum VehicleType
{
    // The default enum value must not silently classify a missing vehicle.
    Unknown = 0,
    Car,
    Motorbike,
    Tractor,
    Emergency,
    Diplomat,
    Foreign,
    Military,
    Other // A known vehicle that has no exemption under this policy.
}
